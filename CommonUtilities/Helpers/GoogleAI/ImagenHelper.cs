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
using GenerativeAI.Clients;
using GenerativeAI.Types;

namespace CommonUtilities.Helpers.GoogleAI;

/// <summary>
///     Helper for Google Imagen AI models.
///     Specialized for generating images from text prompts.
///     Reference: https://github.com/gunpal5/google_generativeai
///     API Docs: https://cloud.google.com/vertex-ai/generative-ai/docs/model-reference/imagen-api
/// </summary>
public class ImagenHelper : IImagenHelper
{
    private readonly ImagenConfig _config;
    private readonly GoogleAi _googleAi;
    private readonly ImagenModel _imageModel;

    /// <summary>
    ///     Initializes the Imagen helper with configuration.
    /// </summary>
    public ImagenHelper(ImagenConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _googleAi = new GoogleAi(_config.ApiKey);
        _imageModel = _googleAi.CreateImageModel(_config.Model);
    }

    /// <summary>
    ///     Generates an image from a text prompt using the Imagen model.
    ///     Returns the first generated image as a byte array.
    ///     Reference:
    ///     https://github.com/gunpal5/google_generativeai/blob/main/src/GenerativeAI/Types/Imagen/GenerateImageResponse.cs
    /// </summary>
    public async Task<byte[]> GenerateImageAsync(string prompt)
    {
        GenerateImageResponse? response = await _imageModel.GenerateImagesAsync(prompt);

        // SDK returns response.Predictions list containing VisionGenerativeModelResult objects
        // Each prediction has BytesBase64Encoded property with the image data
        if (response?.Predictions != null && response.Predictions.Count > 0)
        {
            VisionGenerativeModelResult? prediction = response.Predictions[0];
            if (!string.IsNullOrEmpty(prediction?.BytesBase64Encoded))
                return Convert.FromBase64String(prediction.BytesBase64Encoded);
        }

        return Array.Empty<byte>();
    }

    /// <summary>
    ///     Generates multiple images from a text prompt using the Imagen model.
    ///     Returns a list of generated images as byte arrays.
    ///     Reference:
    ///     https://github.com/gunpal5/google_generativeai/blob/main/src/GenerativeAI/Types/Imagen/GenerateImageResponse.cs
    /// </summary>
    public async Task<List<byte[]>> GenerateImagesAsync(string prompt, int count = 1)
    {
        List<byte[]> images = new();
        GenerateImageResponse? response = await _imageModel.GenerateImagesAsync(prompt);

        // SDK returns response.Predictions list containing VisionGenerativeModelResult objects
        if (response?.Predictions != null && response.Predictions.Count > 0)
            foreach (VisionGenerativeModelResult? prediction in response.Predictions.Take(count))
                if (!string.IsNullOrEmpty(prediction?.BytesBase64Encoded))
                    images.Add(Convert.FromBase64String(prediction.BytesBase64Encoded));

        return images;
    }
}