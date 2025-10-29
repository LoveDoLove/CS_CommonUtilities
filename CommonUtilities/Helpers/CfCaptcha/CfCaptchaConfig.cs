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

namespace CommonUtilities.Helpers.CfCaptcha;

/// <summary>
///     Represents the configuration settings for Cloudflare Turnstile CAPTCHA integration.
/// </summary>
public class CfCaptchaConfig
{
    /// <summary>
    ///     Gets or sets the site key provided by Cloudflare for the CAPTCHA widget.
    /// </summary>
    public string SiteKey { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the secret key used for server-side CAPTCHA verification.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
}