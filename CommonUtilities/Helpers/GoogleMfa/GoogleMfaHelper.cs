// Copyright 2025 LoveDoLove
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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