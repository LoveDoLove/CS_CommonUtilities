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

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Helpers.CfCaptcha;

/// <summary>
///     Defines methods for rendering and verifying Cloudflare Turnstile CAPTCHA in ASP.NET Core applications.
/// </summary>
public interface ICfCaptchaHelper
{
    /// <summary>
    ///     Asynchronously verifies the CAPTCHA token with Cloudflare Turnstile service.
    /// </summary>
    /// <param name="token">The CAPTCHA response token from the client.</param>
    /// <returns>True if the CAPTCHA is valid; otherwise, false.</returns>
    Task<bool> VerifyCaptchaAsync(string token);

    /// <summary>
    ///     Generates the HTML markup required to render the Cloudflare Turnstile CAPTCHA widget.
    /// </summary>
    /// <returns>An <see cref="IHtmlContent" /> containing the CAPTCHA widget HTML.</returns>
    IHtmlContent GetCaptchaHtml();

    /// <summary>
    ///     Validates the CAPTCHA response from the specified HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request containing the CAPTCHA response.</param>
    /// <returns>True if the CAPTCHA response is present and valid; otherwise, false.</returns>
    bool IsCaptchaResponseValid(HttpRequest request);
}