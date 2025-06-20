using Google.Authenticator;

namespace CommonUtilities.Helpers.GoogleMfa;

public class GoogleMfaHelper : IGoogleMfaHelper
{
    private readonly TwoFactorAuthenticator _twoFactorAuthenticator = new();

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

    public bool ValidateMfa(string secretKey, string code)
    {
        return _twoFactorAuthenticator.ValidateTwoFactorPIN(secretKey, code);
    }
}