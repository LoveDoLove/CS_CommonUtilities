using System.Text.Json.Serialization;

namespace CommonUtilities.Helpers.CfCaptcha;

public class CfCaptchaResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }

    // Cloudflare's response can include other fields like 'error-codes', 'challenge_ts', 'hostname', 'action', 'cdata'.
    // These can be added here if needed for more detailed logging or handling.
    // Example:
    // [JsonPropertyName("error-codes")]
    // public List<string>? ErrorCodes { get; set; }
    //
    // [JsonPropertyName("hostname")]
    // public string? Hostname { get; set; }
}