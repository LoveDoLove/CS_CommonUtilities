using System.Text.Json;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Services.CfCaptcha;

/// <summary>
///     Service for verifying Cloudflare Turnstile CAPTCHA challenges.
/// </summary>
public class CfCaptchaService : ICfCaptchaService
{
    private const string CfCaptchaUrl = "https://challenges.cloudflare.com/turnstile/v0/siteverify";
    private const string CfCaptchaHtml = "<div class='cf-turnstile' data-sitekey='CF_CAPTCHA_SITE_KEY'></div>";
    private const string CfTurnstileResponse = "cf-turnstile-response";
    private static readonly HttpClient httpClient = new();

    private readonly CfCaptchaConfig _cfCaptcha;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CfCaptchaService" /> class.
    /// </summary>
    /// <param name="cfCaptcha">The Cloudflare CAPTCHA configuration containing the site key and secret key.</param>
    public CfCaptchaService(CfCaptchaConfig cfCaptcha)
    {
        _cfCaptcha = cfCaptcha;
    }

    /// <summary>
    ///     Verifies a Cloudflare Turnstile CAPTCHA token asynchronously.
    /// </summary>
    /// <param name="token">The CAPTCHA token received from the client.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of true if the token is valid, false otherwise.</returns>
    public async Task<bool> VerifyCaptchaAsync(string token)
    {
        FormUrlEncodedContent content = new([
            new KeyValuePair<string, string>("secret", _cfCaptcha.SecretKey),
            new KeyValuePair<string, string>("response", token)
        ]);
        HttpResponseMessage response = await httpClient.PostAsync(CfCaptchaUrl, content);
        response.EnsureSuccessStatusCode(); // Throw if not successful
        string responseContent = await response.Content.ReadAsStringAsync();
        CfCaptchaResponse? captchaResponse = JsonSerializer.Deserialize<CfCaptchaResponse>(responseContent);
        return captchaResponse != null && captchaResponse.Success;
    }


    /// <summary>
    ///     Gets the HTML content for rendering the Cloudflare Turnstile CAPTCHA widget.
    ///     The site key from the configuration will be injected into the HTML.
    /// </summary>
    /// <returns>An <see cref="IHtmlContent" /> object containing the CAPTCHA widget HTML.</returns>
    public IHtmlContent GetCaptchaHtml()
    {
        return new HtmlString(CfCaptchaHtml.Replace("CF_CAPTCHA_SITE_KEY", _cfCaptcha.SiteKey));
    }

    /// <summary>
    ///     Checks if the CAPTCHA response in the current HTTP request is valid.
    ///     It extracts the 'cf-turnstile-response' token from the form data and verifies it.
    /// </summary>
    /// <param name="request">The current HttpRequest.</param>
    /// <returns>True if the CAPTCHA response is valid, false otherwise.</returns>
    public bool IsCaptchaResponseValid(HttpRequest request)
    {
        string? turnstileResponse = request.Form[CfTurnstileResponse];
        if (string.IsNullOrEmpty(turnstileResponse)) return false;
        return VerifyCaptchaAsync(turnstileResponse).GetAwaiter().GetResult();
    }
}