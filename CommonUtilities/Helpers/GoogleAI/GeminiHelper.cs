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

using System.Text;
using System.Text.Json;
using GenerativeAI;
using GenerativeAI.Types;

namespace CommonUtilities.Helpers.GoogleAI;

/// <summary>
///     Helper for Google Gemini AI models.
///     Supports text generation, chat, streaming, and multimodal input (text + images/files).
///     Includes grounding with Google Search for web-based information retrieval.
///     Reference: https://github.com/gunpal5/google_generativeai
/// </summary>
public class GeminiHelper : IGeminiHelper
{
    private const string GeminiApiUrl = "https://generativelanguage.googleapis.com/v1beta/models";
    private readonly GeminiConfig _config;
    private readonly GoogleAi _googleAi;
    private readonly HttpClient _httpClient;
    private readonly GenerativeModel _model;

    /// <summary>
    ///     Initializes the Gemini helper with configuration.
    /// </summary>
    public GeminiHelper(GeminiConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _googleAi = new GoogleAi(_config.ApiKey);
        _model = _googleAi.CreateGenerativeModel(_config.Model);
        _httpClient = new HttpClient();
    }

    /// <summary>
    ///     Generates text content from a prompt using the configured Gemini model.
    /// </summary>
    public async Task<string> GenerateTextAsync(string prompt)
    {
        GenerateContentResponse? response = await _model.GenerateContentAsync(prompt);
        return response?.Text() ?? string.Empty;
    }

    /// <summary>
    ///     Generates text with grounding enabled (Google Search).
    ///     This allows the model to search the web for current and factual information.
    /// </summary>
    /// <param name="prompt">The prompt to send to the model</param>
    /// <returns>Tuple of (response text, grounding metadata if available)</returns>
    public async Task<(string responseText, GroundingMetadata? metadata)> GenerateTextWithGroundingAsync(string prompt)
    {
        try
        {
            // Only use grounding if enabled in config
            if (!_config.EnableGrounding)
                return (await GenerateTextAsync(prompt), null);

            // Use REST API for grounding support
            return await CallGeminiWithGroundingAsync(prompt);
        }
        catch (Exception ex)
        {
            // If grounding fails, fall back to regular generation
            Console.WriteLine($"Grounding error: {ex.Message}. Falling back to regular generation.");
            return (await GenerateTextAsync(prompt), null);
        }
    }

    /// <summary>
    ///     Starts a chat session and sends a message.
    /// </summary>
    public async Task<string> SendChatMessageAsync(string message)
    {
        ChatSession chat = _model.StartChat();
        GenerateContentResponse? response = await chat.GenerateContentAsync(message);
        return response?.Text() ?? string.Empty;
    }

    /// <summary>
    ///     Streams content from a prompt (text streaming).
    /// </summary>
    public async IAsyncEnumerable<string> StreamTextAsync(string prompt)
    {
        await foreach (GenerateContentResponse? chunk in _model.StreamContentAsync(prompt))
            yield return chunk?.Text() ?? string.Empty;
    }

    /// <summary>
    ///     Sends a multimodal request (text + file/image as input).
    ///     Gemini analyzes the image/file and generates text response.
    ///     Reference: https://github.com/gunpal5/google_generativeai#multimodal
    /// </summary>
    public async Task<string> GenerateMultimodalContentAsync(string prompt, string filePath)
    {
        GenerateContentRequest request = new();
        request.AddText(prompt);
        request.AddInlineFile(filePath);
        GenerateContentResponse? response = await _model.GenerateContentAsync(request);
        return response?.Text() ?? string.Empty;
    }

    /// <summary>
    ///     Gets model information by ID. Not supported in current SDK.
    /// </summary>
    public Task<string> GetModelInfoAsync(string modelId)
    {
        throw new NotSupportedException(
            "Model info retrieval is not supported by the current Google_GenerativeAI SDK.");
    }

    /// <summary>
    ///     Calls the Gemini API with grounding tools via REST API.
    ///     This method bypasses the SDK to access grounding features directly.
    /// </summary>
    private async Task<(string responseText, GroundingMetadata? metadata)> CallGeminiWithGroundingAsync(string prompt)
    {
        try
        {
            // Build the request body with grounding tools
            object requestBody = BuildGroundingRequest(prompt);

            // Extract model name without "models/" prefix if present
            string modelId = _config.Model.Replace("models/", "");
            string url = $"{GeminiApiUrl}/{modelId}:generateContent?key={_config.ApiKey}";

            // Make the API call
            StringContent content = new(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            JsonDocument jsonDocument = JsonDocument.Parse(responseContent);
            JsonElement root = jsonDocument.RootElement;

            // Extract the text response
            string responseText = string.Empty;
            if (root.TryGetProperty("candidates", out JsonElement candidates) && candidates.GetArrayLength() > 0)
            {
                JsonElement firstCandidate = candidates[0];
                if (firstCandidate.TryGetProperty("content", out JsonElement contentElement) &&
                    contentElement.TryGetProperty("parts", out JsonElement parts) && parts.GetArrayLength() > 0)
                    if (parts[0].TryGetProperty("text", out JsonElement textElement))
                        responseText = textElement.GetString() ?? string.Empty;

                // Extract grounding metadata if available
                GroundingMetadata? metadata = ExtractGroundingMetadata(firstCandidate);
                return (responseText, metadata);
            }

            return (responseText, null);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP error in grounding call: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    ///     Builds the request body for Gemini API with grounding tools.
    /// </summary>
    private object BuildGroundingRequest(string prompt)
    {
        // Build tools array based on grounding mode
        List<object> tools = new();

        if (_config.GroundingMode == "DYNAMIC")
            // Dynamic grounding with threshold
            tools.Add(new
            {
                google_search_retrieval = new
                {
                    dynamic_retrieval_config = new
                    {
                        mode = "MODE_DYNAMIC",
                        dynamic_threshold = _config.GroundingThreshold
                    }
                }
            });
        else if (_config.GroundingMode == "ALWAYS")
            // Always perform web search
            tools.Add(new { google_search = new { } });

        return new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            },
            tools
        };
    }

    /// <summary>
    ///     Extracts grounding metadata from the Gemini API response.
    /// </summary>
    private GroundingMetadata? ExtractGroundingMetadata(JsonElement candidate)
    {
        if (!candidate.TryGetProperty("groundingMetadata", out JsonElement metadata))
            return null;

        GroundingMetadata groundingData = new();

        // Extract search queries
        if (metadata.TryGetProperty("searchQueries", out JsonElement queries))
            foreach (JsonElement query in queries.EnumerateArray())
                if (query.TryGetProperty("text", out JsonElement queryText))
                    groundingData.SearchQueries.Add(new SearchQuery
                    {
                        Text = queryText.GetString() ?? string.Empty
                    });

        // Extract web results
        if (metadata.TryGetProperty("webResults", out JsonElement results))
            foreach (JsonElement result in results.EnumerateArray())
            {
                WebResult webResult = new();
                if (result.TryGetProperty("url", out JsonElement url))
                    webResult.Url = url.GetString() ?? string.Empty;
                if (result.TryGetProperty("title", out JsonElement title))
                    webResult.Title = title.GetString() ?? string.Empty;
                if (result.TryGetProperty("snippet", out JsonElement snippet))
                    webResult.Snippet = snippet.GetString() ?? string.Empty;
                groundingData.WebResults.Add(webResult);
            }

        // Extract citations
        if (metadata.TryGetProperty("citations", out JsonElement citations))
            foreach (JsonElement citation in citations.EnumerateArray())
            {
                Citation citationData = new();
                if (citation.TryGetProperty("startIndex", out JsonElement startIndex))
                    citationData.StartIndex = startIndex.GetInt32();
                if (citation.TryGetProperty("endIndex", out JsonElement endIndex))
                    citationData.EndIndex = endIndex.GetInt32();
                if (citation.TryGetProperty("uri", out JsonElement uri))
                    citationData.Uri = uri.GetString() ?? string.Empty;
                groundingData.Citations.Add(citationData);
            }

        return groundingData.SearchQueries.Count > 0 || groundingData.WebResults.Count > 0
            ? groundingData
            : null;
    }
}

/// <summary>
///     Represents grounding metadata returned from Gemini when using grounding tools.
///     Contains information about web search queries, results, and citations.
/// </summary>
public class GroundingMetadata
{
    /// <summary>
    ///     Search queries generated and executed by the model.
    /// </summary>
    public List<SearchQuery> SearchQueries { get; set; } = new();

    /// <summary>
    ///     Web search results retrieved for grounding.
    /// </summary>
    public List<WebResult> WebResults { get; set; } = new();

    /// <summary>
    ///     Citation information embedded in the response.
    /// </summary>
    public List<Citation> Citations { get; set; } = new();
}

/// <summary>
///     Represents a search query generated by the model for grounding.
/// </summary>
public class SearchQuery
{
    public string Text { get; set; } = string.Empty;
}

/// <summary>
///     Represents a web search result used for grounding.
/// </summary>
public class WebResult
{
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Snippet { get; set; } = string.Empty;
}

/// <summary>
///     Represents a citation in the grounded response.
/// </summary>
public class Citation
{
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public string Uri { get; set; } = string.Empty;
}