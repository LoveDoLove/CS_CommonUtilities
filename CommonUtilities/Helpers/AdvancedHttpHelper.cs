using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using Serilog;

namespace CommonUtilities.Helpers
{
    /// <summary>
    /// Provides advanced HTTP client functionalities, including client caching, request execution with retries,
    /// and simplified methods for common JSON-based GET and POST operations.
    /// It uses RestSharp for making HTTP requests.
    /// </summary>
    public static class AdvancedHttpHelper
    {
        private static readonly Dictionary<string, RestClient> _clientCache = new();
        private static readonly object _lock = new(); // Used for thread-safe access to _clientCache

        /// <summary>
        /// Default timeout for HTTP operations in seconds.
        /// </summary>
        private const int DefaultTimeoutSeconds = 30;
        /// <summary>
        /// Default User-Agent string used if none is provided.
        /// </summary>
        private const string DefaultUserAgent = "CommonUtilities.AdvancedHttpHelper/1.0";

        /// <summary>
        /// Gets a cached or new <see cref="RestClient"/> configured for a specific base URL, User-Agent, and timeout.
        /// Caching helps in reusing client instances, which can improve performance by reusing connections.
        /// </summary>
        /// <param name="baseUrl">The base URL for the API client. This URL will be normalized (trailing slash removed).</param>
        /// <param name="userAgent">Optional User-Agent string for the HTTP requests. If null or empty, a default User-Agent will be used.</param>
        /// <param name="timeoutSeconds">Optional timeout in seconds for HTTP requests. Defaults to <see cref="DefaultTimeoutSeconds"/> (30 seconds).</param>
        /// <returns>A configured <see cref="RestClient"/> instance.</returns>
        public static RestClient GetClient(string baseUrl, string? userAgent = null, int? timeoutSeconds = null)
        {
            string normalizedBaseUrl = NormalizeUrl(baseUrl);
            // Create a unique key for caching based on URL, User-Agent, and timeout
            string clientKey = $"{normalizedBaseUrl}_{userAgent ?? DefaultUserAgent}_{timeoutSeconds ?? DefaultTimeoutSeconds}";

            lock (_lock) // Ensure thread-safe access to the cache
            {
                if (_clientCache.TryGetValue(clientKey, out var cachedClient))
                {
                    Log.Debug("Returning cached RestClient for {ClientKey}", clientKey);
                    return cachedClient;
                }

                // Client not found in cache, create a new one
                var options = new RestClientOptions(normalizedBaseUrl)
                {
                    MaxTimeout = (timeoutSeconds ?? DefaultTimeoutSeconds) * 1000, // Timeout in milliseconds
                    ThrowOnAnyError = false, // Allows handling errors manually after execution
                    UserAgent = string.IsNullOrEmpty(userAgent) ? DefaultUserAgent : userAgent,
                };

                var newClient = new RestClient(options);
                _clientCache[clientKey] = newClient; // Add the new client to the cache

                Log.Information("Created and cached new RestClient for {BaseUrl} with User-Agent {UserAgent} and Timeout {Timeout}s",
                    normalizedBaseUrl, options.UserAgent, options.MaxTimeout / 1000);
                return newClient;
            }
        }

        /// <summary>
        /// Executes a <see cref="RestRequest"/> with automatic retry logic for transient failures (e.g., network issues, rate limits, server errors).
        /// Implements an exponential backoff strategy for retries.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response content to.</typeparam>
        /// <param name="client">The <see cref="RestClient"/> instance to use for executing the request.</param>
        /// <param name="request">The <see cref="RestRequest"/> to execute.</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to cancel the request and retries.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation, which wraps the <see cref="RestResponse{T}"/>.</returns>
        /// <exception cref="HttpRequestException">Thrown if the request fails after all retries for reasons other than HTTP errors handled by RestSharp (e.g. network connectivity issues).</exception>
        public static async Task<RestResponse<T>> ExecuteWithRetryAsync<T>(
            RestClient client,
            RestRequest request,
            CancellationToken cancellationToken = default)
        {
            int maxRetries = 3; // Maximum number of retry attempts
            int retryCount = 0; // Current retry attempt
            TimeSpan delay = TimeSpan.FromSeconds(1); // Initial delay before the first retry

            while (true) // Loop indefinitely until success, max retries exceeded, or unrecoverable error
            {
                try
                {
                    Log.Debug("Executing request to {Resource}, Attempt {AttemptNumber}", request.Resource, retryCount + 1);
                    var response = await client.ExecuteAsync<T>(request, cancellationToken);

                    // If the request was successful, return the response immediately
                    if (response.IsSuccessful)
                    {
                        Log.Information("Request to {Resource} succeeded with status {StatusCode}", request.Resource, response.StatusCode);
                        return response;
                    }

                    // Handle specific HTTP status codes that warrant a retry
                    if (response.StatusCode == HttpStatusCode.TooManyRequests) // HTTP 429
                    {
                        retryCount++;
                        if (retryCount <= maxRetries)
                        {
                            Log.Warning("Rate limit hit for {Resource}. Waiting {DelayMs}ms before retry {RetryCount}/{MaxRetries}. Status: {StatusCode}",
                                request.Resource, delay.TotalMilliseconds, retryCount, maxRetries, response.StatusCode);
                            await Task.Delay(delay, cancellationToken);
                            delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * 2); // Exponential backoff: double the delay
                            continue; // Continue to the next iteration to retry
                        }
                    }
                    else if (IsTransientError(response.StatusCode)) // Check for other transient errors (e.g., 5xx, timeout)
                    {
                        retryCount++;
                        if (retryCount <= maxRetries)
                        {
                            Log.Warning("Transient error {StatusCode} for {Resource}. Waiting {DelayMs}ms before retry {RetryCount}/{MaxRetries}",
                                response.StatusCode, request.Resource, delay.TotalMilliseconds, retryCount, maxRetries);
                            await Task.Delay(delay, cancellationToken);
                            delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * 2); // Exponential backoff
                            continue; // Continue to the next iteration to retry
                        }
                    }

                    // If not successful and not a retriable error, or max retries exceeded, log and return the failed response
                    Log.Error("API request to {Resource} failed with status {StatusCode}: {ErrorMessage}. Content: {Content}",
                        request.Resource, response.StatusCode, response.ErrorMessage ?? "N/A", response.Content ?? "N/A");
                    return response; // Return the failed response for the caller to handle
                }
                catch (OperationCanceledException ex)
                {
                    // If the operation was cancelled (e.g., via CancellationToken), log and rethrow
                    Log.Warning(ex, "Operation cancelled for {Resource} during attempt {AttemptNumber}", request.Resource, retryCount + 1);
                    throw;
                }
                catch (Exception ex) when (!(ex is HttpRequestException)) // Catch other exceptions (e.g., network issues not resulting in an HTTP response)
                {
                    retryCount++;
                    if (retryCount <= maxRetries)
                    {
                        Log.Warning(ex, "Network or unexpected error for {Resource}. Waiting {DelayMs}ms before retry {RetryCount}/{MaxRetries}",
                            request.Resource, delay.TotalMilliseconds, retryCount, maxRetries);
                        await Task.Delay(delay, cancellationToken);
                        delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * 2); // Exponential backoff
                        continue; // Continue to the next iteration to retry
                    }

                    // If max retries exceeded for other exceptions, log and throw a new HttpRequestException
                    Log.Error(ex, "API request to {Resource} failed after {RetryCount} retries due to network or unexpected error.", request.Resource, retryCount);
                    throw new HttpRequestException($"Request to {request.Resource} failed after {retryCount} retries.", ex);
                }
            }
        }

        /// <summary>
        /// Makes an HTTP GET request to the specified endpoint and deserializes the JSON response.
        /// Uses <see cref="ExecuteWithRetryAsync{T}"/> for robust execution.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the JSON response to.</typeparam>
        /// <param name="baseUrl">The base URL of the API.</param>
        /// <param name="endpoint">The specific endpoint for the request (e.g., "/users/1").</param>
        /// <param name="headers">Optional dictionary of custom headers to include in the request.</param>
        /// <param name="parameters">Optional dictionary of query parameters to include in the request URL.</param>
        /// <param name="userAgent">Optional User-Agent string. If null, the client's default or the helper's default will be used.</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.
        /// The task result contains the deserialized object of type <typeparamref name="T"/>,
        /// or default(<typeparamref name="T"/>) if the request fails, is unsuccessful, or the response data is null.</returns>
        public static async Task<T?> GetJsonAsync<T>(
            string baseUrl,
            string endpoint,
            Dictionary<string, string>? headers = null,
            Dictionary<string, string>? parameters = null,
            string? userAgent = null,
            CancellationToken cancellationToken = default)
        {
            var client = GetClient(baseUrl, userAgent);
            var request = new RestRequest(endpoint, Method.Get);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddQueryParameter(param.Key, param.Value);
                }
            }

            var response = await ExecuteWithRetryAsync<T>(client, request, cancellationToken);
            if (response.IsSuccessful && response.Data != null)
            {
                return response.Data;
            }

            Log.Warning("GetJsonAsync for {BaseUrl}{Endpoint} failed or returned null data. Status: {StatusCode}, Error: {ErrorMessage}",
                baseUrl, endpoint, response.StatusCode, response.ErrorMessage ?? "N/A");
            return default; // Return default(T) if not successful or data is null
        }

        /// <summary>
        /// Makes an HTTP POST request to the specified endpoint with a JSON payload and deserializes the JSON response.
        /// Uses <see cref="ExecuteWithRetryAsync{T}"/> for robust execution.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the JSON response to.</typeparam>
        /// <param name="baseUrl">The base URL of the API.</param>
        /// <param name="endpoint">The specific endpoint for the request (e.g., "/users").</param>
        /// <param name="payload">The object to serialize as JSON and send as the request body.</param>
        /// <param name="headers">Optional dictionary of custom headers to include in the request.</param>
        /// <param name="userAgent">Optional User-Agent string. If null, the client's default or the helper's default will be used.</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.
        /// The task result contains the deserialized object of type <typeparamref name="T"/>,
        /// or default(<typeparamref name="T"/>) if the request fails, is unsuccessful, or the response data is null.</returns>
        public static async Task<T?> PostJsonAsync<T>(
            string baseUrl,
            string endpoint,
            object payload,
            Dictionary<string, string>? headers = null,
            string? userAgent = null,
            CancellationToken cancellationToken = default)
        {
            var client = GetClient(baseUrl, userAgent);
            var request = new RestRequest(endpoint, Method.Post);
            request.AddJsonBody(payload); // Automatically serializes the payload to JSON

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            var response = await ExecuteWithRetryAsync<T>(client, request, cancellationToken);
            if (response.IsSuccessful && response.Data != null)
            {
                return response.Data;
            }

            Log.Warning("PostJsonAsync for {BaseUrl}{Endpoint} failed or returned null data. Status: {StatusCode}, Error: {ErrorMessage}",
                baseUrl, endpoint, response.StatusCode, response.ErrorMessage ?? "N/A");
            return default; // Return default(T) if not successful or data is null
        }

        /// <summary>
        /// Makes an HTTP request (GET, POST, PUT, DELETE) to the specified endpoint and deserializes the JSON response into a list of objects.
        /// Uses <see cref="ExecuteWithRetryAsync{T}"/> for robust execution.
        /// </summary>
        /// <typeparam name="T">The type of objects in the list to deserialize the JSON response to.</typeparam>
        /// <param name="baseUrl">The base URL of the API.</param>
        /// <param name="endpoint">The specific endpoint for the request.</param>
        /// <param name="method">The HTTP method to use (e.g., <see cref="System.Net.Http.HttpMethod.Get"/>, <see cref="System.Net.Http.HttpMethod.Post"/>). Defaults to GET.</param>
        /// <param name="headers">Optional dictionary of custom headers to include in the request.</param>
        /// <param name="payload">Optional payload for POST/PUT requests. Will be serialized to JSON if the method is POST or PUT.</param>
        /// <param name="userAgent">Optional User-Agent string. If null, the client's default or the helper's default will be used.</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.
        /// The task result contains the deserialized list of objects of type <typeparamref name="T"/>,
        /// or an empty list if the request fails, is unsuccessful, or the response data is null or empty.</returns>
        public static async Task<List<T>> GetJsonListAsync<T>(
            string baseUrl,
            string endpoint,
            System.Net.Http.HttpMethod? method = null,
            Dictionary<string, string>? headers = null,
            object? payload = null,
            string? userAgent = null,
            CancellationToken cancellationToken = default)
        {
            var client = GetClient(baseUrl, userAgent);
            // Determine RestSharp method from HttpMethod
            var restMethod = (method ?? System.Net.Http.HttpMethod.Get).Method.ToUpperInvariant() switch
            {
                "POST" => Method.Post,
                "PUT" => Method.Put,
                "DELETE" => Method.Delete,
                "GET" => Method.Get,
                // Add other methods as needed, or throw for unsupported ones
                _ => Method.Get, // Default to GET for simplicity or throw an ArgumentException
            };
            var request = new RestRequest(endpoint, restMethod);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            // Add JSON body only for methods that typically have one
            if (payload != null && (restMethod == Method.Post || restMethod == Method.Put))
            {
                request.AddJsonBody(payload);
            }

            var response = await ExecuteWithRetryAsync<List<T>>(client, request, cancellationToken);
            if (response.IsSuccessful && response.Data != null)
            {
                return response.Data;
            }

            Log.Warning("GetJsonListAsync for {BaseUrl}{Endpoint} (Method: {Method}) failed or returned null/empty data. Status: {StatusCode}, Error: {ErrorMessage}",
                baseUrl, endpoint, restMethod, response.StatusCode, response.ErrorMessage ?? "N/A");
            return new List<T>(); // Return an empty list if not successful or data is null
        }

        /// <summary>
        /// Determines if an HTTP status code represents a transient error that might be resolved by retrying the request.
        /// Transient errors include timeouts, rate limits (though handled separately), and server-side errors (5xx).
        /// </summary>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> to check.</param>
        /// <returns>True if the status code indicates a transient error; otherwise, false.</returns>
        private static bool IsTransientError(HttpStatusCode? statusCode)
        {
            if (!statusCode.HasValue) return false; // Cannot determine if null, treat as non-transient

            return statusCode == HttpStatusCode.RequestTimeout ||       // HTTP 408
                   statusCode == HttpStatusCode.BadGateway ||           // HTTP 502
                   statusCode == HttpStatusCode.ServiceUnavailable ||   // HTTP 503
                   statusCode == HttpStatusCode.GatewayTimeout ||       // HTTP 504
                   (int)statusCode.Value >= 500; // Any 5xx server error
        }

        /// <summary>
        /// Normalizes a URL by trimming trailing slashes. This helps in consistent client caching.
        /// </summary>
        /// <param name="url">The URL string to normalize.</param>
        /// <returns>The normalized URL string, or an empty string if the input is null or empty.</returns>
        private static string NormalizeUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }
            return url.TrimEnd('/'); // Remove trailing slash for consistency
        }

        /// <summary>
        /// Cleans up all cached <see cref="RestClient"/> instances by disposing them and clearing the cache.
        /// This should be called on application shutdown or when the clients are no longer needed to release resources.
        /// </summary>
        public static void Cleanup()
        {
            lock (_lock) // Ensure thread-safe access to the cache
            {
                if (_clientCache.Count == 0)
                {
                    Log.Debug("AdvancedHttpHelper client cache is already empty. No cleanup needed.");
                    return;
                }

                Log.Information("Cleaning up {Count} cached RestClient instances.", _clientCache.Count);
                foreach (var clientEntry in _clientCache)
                {
                    try
                    {
                        clientEntry.Value.Dispose();
                        Log.Debug("Disposed RestClient for key {ClientKey}", clientEntry.Key);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error disposing RestClient for key {ClientKey}", clientEntry.Key);
                    }
                }
                _clientCache.Clear();
                Log.Information("AdvancedHttpHelper client cache cleared successfully.");
            }
        }
    }
}