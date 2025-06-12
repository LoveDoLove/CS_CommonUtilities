using System.Text.Json.Serialization;

namespace CommonUtilities.Services.CfCaptcha;

/// <summary>
///     Represents the response from the Cloudflare Turnstile CAPTCHA verification endpoint.
/// </summary>
public class CfCaptchaResponse
{
    /// <summary>
    ///     Gets or sets a value indicating whether the CAPTCHA verification was successful.
    ///     This property is mapped from the "success" field in the JSON response from Cloudflare.
    /// </summary>
    /// <value><c>true</c> if the CAPTCHA token was valid; otherwise, <c>false</c>.</value>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    // Cloudflare's response can include other fields like 'error-codes', 'challenge_ts', 'hostname', 'action', 'cdata'.
    // These can be added here if needed for more detailed logging or handling.
    // Example:
    // [JsonPropertyName("error-codes")]
    // public List<string>? ErrorCodes { get; set; }
    //
    // [JsonPropertyName("hostname")]
    // public string? Hostname { get; set; }
}