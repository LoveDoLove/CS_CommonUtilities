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

using System.Text.Json;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Helpers.CfCaptcha;

/// <summary>
///     Provides helper methods for integrating and verifying Cloudflare Turnstile CAPTCHA in ASP.NET Core applications.
/// </summary>
public class CfCaptchaHelper : ICfCaptchaHelper
{
    private const string CfCaptchaUrl = "https://challenges.cloudflare.com/turnstile/v0/siteverify";
    private const string CfCaptchaHtml = "<div class='cf-turnstile' data-sitekey='CF_CAPTCHA_SITE_KEY'></div>";
    private const string CfTurnstileResponse = "cf-turnstile-response";
    private static readonly HttpClient HttpClient = new();

    private readonly CfCaptchaConfig _cfCaptcha;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CfCaptchaHelper" /> class with the specified configuration.
    /// </summary>
    /// <param name="cfCaptcha">The Cloudflare CAPTCHA configuration.</param>
    public CfCaptchaHelper(CfCaptchaConfig cfCaptcha)
    {
        _cfCaptcha = cfCaptcha;
    }

    /// <summary>
    ///     Asynchronously verifies the CAPTCHA token with Cloudflare Turnstile service.
    /// </summary>
    /// <param name="token">The CAPTCHA response token from the client.</param>
    /// <returns>True if the CAPTCHA is valid; otherwise, false.</returns>
    public async Task<bool> VerifyCaptchaAsync(string token)
    {
        FormUrlEncodedContent content = new([
            new KeyValuePair<string, string>("secret", _cfCaptcha.SecretKey),
            new KeyValuePair<string, string>("response", token)
        ]);
        HttpResponseMessage response = await HttpClient.PostAsync(CfCaptchaUrl, content);
        response.EnsureSuccessStatusCode(); // Throw if not successful
        string responseContent = await response.Content.ReadAsStringAsync();
        CfCaptchaResponse? captchaResponse = JsonSerializer.Deserialize<CfCaptchaResponse>(responseContent);
        return captchaResponse != null && captchaResponse.Success;
    }

    /// <summary>
    ///     Generates the HTML markup required to render the Cloudflare Turnstile CAPTCHA widget.
    /// </summary>
    /// <returns>An <see cref="IHtmlContent" /> containing the CAPTCHA widget HTML.</returns>
    public IHtmlContent GetCaptchaHtml()
    {
        return new HtmlString(CfCaptchaHtml.Replace("CF_CAPTCHA_SITE_KEY", _cfCaptcha.SiteKey));
    }

    /// <summary>
    ///     Validates the CAPTCHA response from the specified HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request containing the CAPTCHA response.</param>
    /// <returns>True if the CAPTCHA response is present and valid; otherwise, false.</returns>
    public bool IsCaptchaResponseValid(HttpRequest request)
    {
        string? turnstileResponse = request.Form[CfTurnstileResponse];
        if (string.IsNullOrEmpty(turnstileResponse)) return false;
        return VerifyCaptchaAsync(turnstileResponse).GetAwaiter().GetResult();
    }
}