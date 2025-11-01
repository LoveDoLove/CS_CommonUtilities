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
///     Configuration for Google Imagen helper.
///     Reference: https://cloud.google.com/vertex-ai/generative-ai/docs/model-reference/imagen-api
/// </summary>
public class ImagenConfig
{
    /// <summary>
    ///     Google API Key for authentication.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    ///     Imagen model name (e.g., "imagen-3.0-generate-002", "imagen-3.0-generate-001").
    /// </summary>
    public string Model { get; set; } = "imagen-3.0-generate-002";

    /// <summary>
    ///     Optional: Project ID for Vertex AI.
    /// </summary>
    public string? ProjectId { get; set; }

    /// <summary>
    ///     Optional: Region for Vertex AI (default: "us-central1").
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    ///     Optional: Use Vertex AI (default: false).
    /// </summary>
    public bool IsVertex { get; set; } = false;
}