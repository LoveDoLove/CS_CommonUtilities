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
///     Interface for Google Imagen helper.
///     Imagen models are specialized for generating images from text prompts.
///     Reference: https://cloud.google.com/vertex-ai/generative-ai/docs/model-reference/imagen-api
/// </summary>
public interface IImagenHelper
{
    /// <summary>
    ///     Generates an image from a text prompt.
    ///     Returns the image as a byte array.
    /// </summary>
    Task<byte[]> GenerateImageAsync(string prompt);

    /// <summary>
    ///     Generates multiple images from a text prompt.
    ///     Returns a list of images as byte arrays.
    /// </summary>
    Task<List<byte[]>> GenerateImagesAsync(string prompt, int count = 1);
}