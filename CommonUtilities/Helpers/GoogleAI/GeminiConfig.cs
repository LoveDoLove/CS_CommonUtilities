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

using System.ComponentModel;

namespace CommonUtilities.Helpers.GoogleAI;

/// <summary>
///     Configuration for Google Gemini helper.
///     Reference: https://github.com/gunpal5/google_generativeai#configuration
/// </summary>
public class GeminiConfig
{
    /// <summary>
    ///     Google API Key for authentication.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    ///     Model name (e.g., "models/gemini-1.5-flash", "models/gemini-2.0-flash-exp").
    /// </summary>
    public string Model { get; set; } = "models/gemini-2.5-flash-lite";

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
    [DefaultValue(false)]
    public bool IsVertex { get; set; } = false;

    /// <summary>
    ///     Optional: Express mode flag.
    /// </summary>
    [DefaultValue(false)]
    public bool ExpressMode { get; set; } = false;

    /// <summary>
    ///     Optional: Enable Google Search grounding for web search capabilities (default: false).
    ///     When enabled, Gemini can search the web to provide current and factual information.
    /// </summary>
    [DefaultValue(false)]
    public bool EnableGrounding { get; set; } = false;

    /// <summary>
    ///     Optional: Dynamic retrieval threshold for grounding (0.0 to 1.0, default: 0.7).
    ///     The model will only perform a web search if its confidence in answering from its own knowledge
    ///     falls below this threshold. Set to 1.0 to always search, or 0.0 to never search.
    /// </summary>
    [DefaultValue(0.7)]
    public double GroundingThreshold { get; set; } = 0.7;

    /// <summary>
    ///     Optional: Grounding mode ("DYNAMIC" for dynamic threshold, "ALWAYS" to always ground).
    ///     Default: "DYNAMIC" - only searches when confidence is below threshold.
    /// </summary>
    [DefaultValue("DYNAMIC")]
    public string GroundingMode { get; set; } = "DYNAMIC";
}