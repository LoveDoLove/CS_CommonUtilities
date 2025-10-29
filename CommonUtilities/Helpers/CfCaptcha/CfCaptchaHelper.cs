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