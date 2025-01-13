using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Interfaces;

public interface ICfCaptchaService
{
    bool VerifyCaptcha(string token);
    IHtmlContent GetCaptchaHtml();
    bool IsCaptchaResponseValid(HttpRequest request);
}