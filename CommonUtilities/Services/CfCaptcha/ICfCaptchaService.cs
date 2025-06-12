using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Services.CfCaptcha;

/// <summary>
///     Defines the contract for a service that handles Cloudflare Turnstile CAPTCHA verification and rendering.
/// </summary>
public interface ICfCaptchaService
{
    /// <summary>
    ///     Verifies a Cloudflare Turnstile CAPTCHA token asynchronously.
    /// </summary>
    /// <param name="token">The CAPTCHA token received from the client-side widget.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains <c>true</c> if the token is valid;
    ///     otherwise, <c>false</c>.
    /// </returns>
    Task<bool> VerifyCaptchaAsync(string token);

    /// <summary>
    ///     Gets the HTML content required to render the Cloudflare Turnstile CAPTCHA widget.
    ///     This typically includes the necessary script tags and div element.
    /// </summary>
    /// <returns>An <see cref="IHtmlContent" /> object containing the HTML for the CAPTCHA widget.</returns>
    IHtmlContent GetCaptchaHtml();

    /// <summary>
    ///     Checks if the CAPTCHA response from the HTTP request is valid.
    ///     This method might involve extracting the token from the request and calling <see cref="VerifyCaptchaAsync" />.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequest" /> containing the CAPTCHA response, typically from a form submission.</param>
    /// <returns><c>true</c> if the CAPTCHA response is present and valid; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     This method might need to become asynchronous for consistency if its implementation involves asynchronous
    ///     operations like calling <see cref="VerifyCaptchaAsync" />.
    /// </remarks>
    bool IsCaptchaResponseValid(HttpRequest request);
}