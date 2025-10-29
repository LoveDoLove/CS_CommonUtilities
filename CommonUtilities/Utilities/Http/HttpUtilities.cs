// Copyright 2025 LoveDoLove
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace CommonUtilities.Utilities.Http;

/// <summary>
///     Provides simple utility methods for making HTTP requests.
///     This class uses a static <see cref="HttpClient" /> instance for efficiency.
///     For more advanced scenarios like custom headers, timeouts, or retry logic, consider using
///     <see cref="AdvancedHttpUtilities" />.
/// </summary>
public static class HttpUtilities
{
    // Using a single static HttpClient instance is generally recommended for performance
    // and to avoid socket exhaustion issues that can occur when creating many HttpClient instances frequently.
    private static readonly HttpClient client = new();

    /// <summary>
    ///     Asynchronously retrieves the HTML content from a specified URL.
    /// </summary>
    /// <param name="url">The URL from which to fetch the HTML content.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the HTML content as a string.
    ///     Returns <see cref="string.Empty" /> if an <see cref="HttpRequestException" /> occurs (e.g., network error,
    ///     non-success status code).
    ///     Consider logging the exception for diagnostics.
    /// </returns>
    /// <remarks>
    ///     This method uses a shared static <see cref="HttpClient" /> instance.
    ///     It calls <see cref="HttpResponseMessage.EnsureSuccessStatusCode()" />, which will throw an
    ///     <see cref="HttpRequestException" />
    ///     if the HTTP response status indicates an error.
    /// </remarks>
    public static async Task<string> GetHtmlWithUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            // Consider logging this or throwing ArgumentNullException
            // Log.Warning("GetHtmlWithUrl called with null or empty URL.");
            return string.Empty;

        try
        {
            // Send a GET request to the specified URL.
            HttpResponseMessage response = await client.GetAsync(url);

            // Throw an exception if the HTTP response status is an error code (e.g., 404 Not Found, 500 Internal Server Error).
            response.EnsureSuccessStatusCode();

            // Read the response content as a string.
            string htmlContent = await response.Content.ReadAsStringAsync();
            return htmlContent;
        }
        catch (HttpRequestException ex)
        {
            // This catch block handles exceptions thrown by GetAsync (e.g., DNS resolution failure, network connection refused)
            // and exceptions thrown by EnsureSuccessStatusCode (e.g., HTTP 4xx or 5xx responses).
            // It's important to log this exception for troubleshooting.
            // For example, using Serilog: Log.Error(ex, "Error fetching HTML from URL: {Url}", url);
            // Depending on the application's requirements, you might want to:
            // 1. Return string.Empty (as currently implemented) - simple error suppression.
            // 2. Rethrow the exception: `throw;` - to let the caller handle it.
            // 3. Throw a custom, more specific exception: `throw new CustomHttpFetchException("Failed to get HTML.", ex);`
            // 4. Return null or a specific error indicator.
            Console.Error.WriteLine($"Error fetching HTML from {url}: {ex.Message}"); // Basic error logging to console
            return string.Empty;
        }
        catch (Exception ex) // Catch other potential exceptions, though HttpRequestException is the most common.
        {
            // Log unexpected exceptions.
            // For example: Log.Error(ex, "An unexpected error occurred while fetching HTML from URL: {Url}", url);
            Console.Error.WriteLine($"An unexpected error occurred fetching HTML from {url}: {ex.Message}");
            return string.Empty;
        }
    }
}