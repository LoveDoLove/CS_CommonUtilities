using System.Text.Json;
using CommonUtilities.Interfaces;
using CommonUtilities.Models;
using CommonUtilities.Models.Response.CfCaptcha;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Services;

public class CfCaptchaService : ICfCaptchaService
{
    private const string CfCaptchaUrl = "https://challenges.cloudflare.com/turnstile/v0/siteverify";
    private const string CfCaptchaHtml = "<div class='cf-turnstile' data-sitekey='CF_CAPTCHA_SITE_KEY'></div>";
    private const string CfTurnstileResponse = "cf-turnstile-response";

    private readonly CfCaptcha _cfCaptcha;

    public CfCaptchaService(CfCaptcha cfCaptcha)
    {
        _cfCaptcha = cfCaptcha;
    }

    public bool VerifyCaptcha(string token)
    {
        using HttpClient client = new();
        FormUrlEncodedContent content = new([
            new KeyValuePair<string, string>("secret", _cfCaptcha.SecretKey),
            new KeyValuePair<string, string>("response", token)
        ]);
        HttpResponseMessage response = client.PostAsync(CfCaptchaUrl, content).Result;
        string responseContent = response.Content.ReadAsStringAsync().Result;
        CfCaptchaResponse? captchaResponse = JsonSerializer.Deserialize<CfCaptchaResponse>(responseContent);
        return captchaResponse != null && captchaResponse.Success;
    }

    public IHtmlContent GetCaptchaHtml()
    {
        return new HtmlString(CfCaptchaHtml.Replace("CF_CAPTCHA_SITE_KEY", _cfCaptcha.SiteKey));
    }

    public bool IsCaptchaResponseValid(HttpRequest request)
    {
        string? turnstileResponse = request.Form[CfTurnstileResponse];
        if (turnstileResponse == null) return false;
        return request.Form.ContainsKey(CfTurnstileResponse) && VerifyCaptcha(turnstileResponse);
    }
}