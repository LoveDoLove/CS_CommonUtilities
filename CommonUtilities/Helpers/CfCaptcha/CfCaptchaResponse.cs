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

using System.Text.Json.Serialization;

namespace CommonUtilities.Helpers.CfCaptcha;

/// <summary>
///     Represents the response from the Cloudflare Turnstile CAPTCHA verification API.
/// </summary>
public class CfCaptchaResponse
{
    /// <summary>
    ///     Gets or sets a value indicating whether the CAPTCHA verification was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    // Cloudflare's response can include other fields like 'error-codes', 'challenge_ts', 'hostname', 'action', 'cdata'.
    // These can be added here if needed for more detailed logging or handling.
    // Example:
    // [JsonPropertyName("error-codes")]
    // public List<string>? ErrorCodes { get; set; }
    //
    // [JsonPropertyName("hostname")]
    // public string? Hostname { get; set; }
}