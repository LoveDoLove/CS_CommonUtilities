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

namespace CommonUtilities.Helpers.GoogleAI;

/// <summary>
///     Interface for Google Gemini helper.
///     Gemini models support text generation, chat, streaming, and multimodal input (text + images/files).
///     Includes support for grounding with Google Search for web-based information.
///     Reference: https://github.com/gunpal5/google_generativeai#usage
/// </summary>
public interface IGeminiHelper
{
    /// <summary>
    ///     Generates text content from a prompt.
    /// </summary>
    Task<string> GenerateTextAsync(string prompt);

    /// <summary>
    ///     Generates text with grounding enabled (Google Search).
    ///     This allows the model to search the web for current and factual information.
    /// </summary>
    /// <param name="prompt">The prompt to send to the model</param>
    /// <returns>Tuple of (response text, grounding metadata if available)</returns>
    Task<(string responseText, GroundingMetadata? metadata)> GenerateTextWithGroundingAsync(string prompt);

    /// <summary>
    ///     Starts a chat session and sends a message.
    /// </summary>
    Task<string> SendChatMessageAsync(string message);

    /// <summary>
    ///     Streams content from a prompt (text streaming).
    /// </summary>
    IAsyncEnumerable<string> StreamTextAsync(string prompt);

    /// <summary>
    ///     Sends a multimodal request (text + file/image as input).
    ///     Gemini analyzes the image/file and generates text response.
    /// </summary>
    Task<string> GenerateMultimodalContentAsync(string prompt, string filePath);

    /// <summary>
    ///     Gets model information by ID. Not supported in current SDK.
    /// </summary>
    Task<string> GetModelInfoAsync(string modelId);
}