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

namespace CommonUtilities.Helpers.GoogleMfa;

/// <summary>
///     Represents the configuration details required for Google Authenticator MFA setup.
/// </summary>
public class GoogleMfaConfig
{
    /// <summary>
    ///     Gets or sets the URL for the QR code image used in authenticator apps.
    /// </summary>
    public string QrCodeUrl { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the manual entry code for authenticator apps.
    /// </summary>
    public string ManualEntryCode { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the secret key used for generating and validating MFA codes.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
}