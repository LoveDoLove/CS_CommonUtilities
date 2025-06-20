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