using Google.Authenticator;
using Microsoft.Extensions.Logging;

namespace CommonUtilities.Services.GoogleMfa;

/// <summary>
///     Service for generating and validating Google Authenticator Multi-Factor Authentication (MFA).
/// </summary>
public class GoogleMfaService : IGoogleMfaService
{
    private readonly ILogger<GoogleMfaService> _logger;
    private readonly TwoFactorAuthenticator _twoFactorAuthenticator = new();

    /// <summary>
    ///     Initializes a new instance of the <see cref="GoogleMfaService" /> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public GoogleMfaService(ILogger<GoogleMfaService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Generates MFA setup information (QR code URL, manual entry code, secret key).
    /// </summary>
    /// <param name="issuer">The issuer name to display in the authenticator app (e.g., your application name).</param>
    /// <param name="email">The user's email address or identifier.</param>
    /// <returns>A <see cref="GoogleMfaConfig" /> object containing the QR code URL, manual entry code, and secret key.</returns>
    public GoogleMfaConfig GenerateMfa(string issuer, string email)
    {
        string secretKey = Guid.NewGuid().ToString().Replace("-", "")[..10];

        SetupCode? setupInfo = _twoFactorAuthenticator.GenerateSetupCode(issuer, email, secretKey, false);

        GoogleMfaConfig googleMfa = new()
        {
            QrCodeUrl = setupInfo.QrCodeSetupImageUrl,
            ManualEntryCode = setupInfo.ManualEntryKey,
            SecretKey = secretKey
        };
        return googleMfa;
    }

    /// <summary>
    ///     Validates an MFA code against a secret key.
    /// </summary>
    /// <param name="secretKey">The secret key used to generate the MFA codes.</param>
    /// <param name="code">The MFA code entered by the user.</param>
    /// <returns>True if the MFA code is valid, false otherwise.</returns>
    public bool ValidateMfa(string secretKey, string code)
    {
        return _twoFactorAuthenticator.ValidateTwoFactorPIN(secretKey, code);
    }
}