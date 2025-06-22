// MIT License
// 
// Copyright (c) 2025 LoveDoLove
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Google.Authenticator;

namespace CommonUtilities.Helpers.GoogleMfa;

/// <summary>
///     Provides helper methods for generating and validating Google Authenticator MFA codes.
/// </summary>
public static class GoogleMfaHelper
{
    private static readonly TwoFactorAuthenticator TwoFactorAuthenticator = new();

    /// <summary>
    ///     Generates a new Google MFA configuration, including secret key, QR code URL, and manual entry code.
    /// </summary>
    /// <param name="issuer">The name of the service or application issuing the MFA.</param>
    /// <param name="email">The user's email address for MFA registration.</param>
    /// <returns>A <see cref="GoogleMfaConfig" /> containing the MFA setup details.</returns>
    public static GoogleMfaConfig GenerateMfa(string issuer, string email)
    {
        string secretKey = Guid.NewGuid().ToString().Replace("-", "")[..10];

        SetupCode? setupInfo = TwoFactorAuthenticator.GenerateSetupCode(issuer, email, secretKey, false);

        GoogleMfaConfig googleMfa = new()
        {
            QrCodeUrl = setupInfo.QrCodeSetupImageUrl,
            ManualEntryCode = setupInfo.ManualEntryKey,
            SecretKey = secretKey
        };
        return googleMfa;
    }

    /// <summary>
    ///     Validates the provided MFA code against the secret key.
    /// </summary>
    /// <param name="secretKey">The secret key associated with the user.</param>
    /// <param name="code">The MFA code to validate.</param>
    /// <returns>True if the code is valid; otherwise, false.</returns>
    public static bool ValidateMfa(string secretKey, string code)
    {
        return TwoFactorAuthenticator.ValidateTwoFactorPIN(secretKey, code);
    }
}