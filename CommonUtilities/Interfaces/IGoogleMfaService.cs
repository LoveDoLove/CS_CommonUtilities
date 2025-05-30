using CommonUtilities.Models;

namespace CommonUtilities.Interfaces;

/// <summary>
///     Defines the contract for a service that handles Google Authenticator Multi-Factor Authentication (MFA).
///     This includes generating MFA setup details and validating MFA codes.
/// </summary>
public interface IGoogleMfaService
{
    /// <summary>
    ///     Generates the necessary details for setting up Google Authenticator MFA.
    /// </summary>
    /// <param name="issuer">
    ///     The name of the issuer (e.g., your application or company name) to be displayed in the
    ///     authenticator app.
    /// </param>
    /// <param name="email">The user's email address or username, used to identify the account in the authenticator app.</param>
    /// <returns>
    ///     A <see cref="GoogleMfa" /> object containing the manual setup key, QR code image URL (or Base64 image), and
    ///     other relevant details.
    /// </returns>
    GoogleMfa GenerateMfa(string issuer, string email);

    /// <summary>
    ///     Validates a Time-based One-Time Password (TOTP) code provided by the user against the stored secret key.
    /// </summary>
    /// <param name="secretKey">The secret key associated with the user's MFA setup.</param>
    /// <param name="code">The TOTP code entered by the user from their authenticator app.</param>
    /// <returns><c>true</c> if the provided code is valid for the given secret key; otherwise, <c>false</c>.</returns>
    bool ValidateMfa(string secretKey, string code);
}