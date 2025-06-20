namespace CommonUtilities.Helpers.GoogleMfa;

public interface IGoogleMfaHelper
{
    GoogleMfaConfig GenerateMfa(string issuer, string email);
    bool ValidateMfa(string secretKey, string code);
}