namespace CommonUtilities.Models;

/// <summary>
/// Represents the details required for setting up Google Authenticator Multi-Factor Authentication (MFA).
/// This includes the QR code for easy setup, a manual entry key, and the secret key.
/// </summary>
public class GoogleMfa
{
    /// <summary>
    /// Gets or sets the URL or Base64 encoded string for the QR code image.
    /// This QR code can be scanned by authenticator apps (like Google Authenticator) to automatically configure MFA.
    /// </summary>
    /// <value>The QR code URL or Base64 image string. Defaults to an empty string.</value>
    public string QrCodeUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the manual entry key (often the same as the secret key, but can be formatted differently for display).
    /// This key can be manually entered into an authenticator app if QR code scanning is not possible.
    /// </summary>
    /// <value>The manual entry key. Defaults to an empty string.</value>
    public string ManualEntryCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the secret key (Base32 encoded) used to generate Time-based One-Time Passwords (TOTP).
    /// This key is shared between the server and the user's authenticator app.
    /// It should be stored securely.
    /// </summary>
    /// <value>The secret key. Defaults to an empty string.</value>
    public string SecretKey { get; set; } = string.Empty;
}