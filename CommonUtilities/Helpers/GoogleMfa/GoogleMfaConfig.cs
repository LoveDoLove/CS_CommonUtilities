namespace CommonUtilities.Helpers.GoogleMfa;

public class GoogleMfaConfig
{
    public string QrCodeUrl { get; set; } = string.Empty;

    public string ManualEntryCode { get; set; } = string.Empty;

    public string SecretKey { get; set; } = string.Empty;
}