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

using GenerativeAI;
using GenerativeAI.Types;

namespace CommonUtilities.Helpers.GoogleAI;

/// <summary>
///     Helper for Google Gemini AI models.
///     Supports text generation, chat, streaming, and multimodal input (text + images/files).
///     Reference: https://github.com/gunpal5/google_generativeai
/// </summary>
public class GeminiHelper : IGeminiHelper
{
    private readonly GeminiConfig _config;
    private readonly GoogleAi _googleAi;
    private readonly GenerativeModel _model;

    /// <summary>
    ///     Initializes the Gemini helper with configuration.
    /// </summary>
    public GeminiHelper(GeminiConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _googleAi = new GoogleAi(_config.ApiKey);
        _model = _googleAi.CreateGenerativeModel(_config.Model);
    }

    /// <summary>
    ///     Generates text content from a prompt using the configured Gemini model.
    /// </summary>
    public async Task<string> GenerateTextAsync(string prompt)
    {
        var response = await _model.GenerateContentAsync(prompt);
        return response?.Text() ?? string.Empty;
    }

    /// <summary>
    ///     Starts a chat session and sends a message.
    /// </summary>
    public async Task<string> SendChatMessageAsync(string message)
    {
        var chat = _model.StartChat();
        var response = await chat.GenerateContentAsync(message);
        return response?.Text() ?? string.Empty;
    }

    /// <summary>
    ///     Streams content from a prompt (text streaming).
    /// </summary>
    public async IAsyncEnumerable<string> StreamTextAsync(string prompt)
    {
        await foreach (var chunk in _model.StreamContentAsync(prompt)) yield return chunk?.Text() ?? string.Empty;
    }

    /// <summary>
    ///     Sends a multimodal request (text + file/image as input).
    ///     Gemini analyzes the image/file and generates text response.
    ///     Reference: https://github.com/gunpal5/google_generativeai#multimodal
    /// </summary>
    public async Task<string> GenerateMultimodalContentAsync(string prompt, string filePath)
    {
        var request = new GenerateContentRequest();
        request.AddText(prompt);
        request.AddInlineFile(filePath);
        var response = await _model.GenerateContentAsync(request);
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
}