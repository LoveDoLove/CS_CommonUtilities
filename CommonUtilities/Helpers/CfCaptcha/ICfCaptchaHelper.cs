using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Helpers.CfCaptcha;

public interface ICfCaptchaHelper
{
    Task<bool> VerifyCaptchaAsync(string token);
    IHtmlContent GetCaptchaHtml();
    bool IsCaptchaResponseValid(HttpRequest request);
}