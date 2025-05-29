namespace CommonUtilities.Models;

/// <summary>
///     Represents the configuration settings for Cloudflare Turnstile CAPTCHA.
///     This class is typically used to store site and secret keys required for CAPTCHA integration.
/// </summary>
public class CfCaptcha
{
    /// <summary>
    ///     Gets or sets the Cloudflare Turnstile site key.
    ///     This key is used in the client-side HTML to render the CAPTCHA widget.
    /// </summary>
    /// <value>The site key. Defaults to an empty string.</value>
    public string SiteKey { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the Cloudflare Turnstile secret key.
    ///     This key is used in server-side verification of the CAPTCHA token.
    /// </summary>
    /// <value>The secret key. Defaults to an empty string.</value>
    public string SecretKey { get; set; } = string.Empty;
}