// MIT License
// 
// Copyright (c) 2025 LoveDoLove
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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