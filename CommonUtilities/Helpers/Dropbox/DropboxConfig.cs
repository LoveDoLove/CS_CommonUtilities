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

namespace CommonUtilities.Helpers.Dropbox;

/// <summary>
///     Dropbox Configuration
/// </summary>
public class DropboxConfig
{
    /// <summary>
    ///     Access Token (short-lived, automatically refreshed via RefreshToken).
    ///     Do not manually set. Used internally by DropboxHelper.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    ///     Refresh Token (long-lived).
    ///     IMPORTANT: Must be set to the value returned as "refresh_token" from the OAuth2 authorization flow response.
    ///     You cannot get a refresh token from the App Console's 'Generate' button or from an access token.
    ///     See Dropbox OAuth guide: https://developers.dropbox.com/oauth-guide
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    ///     Dropbox App Client ID (App Key from Dropbox App Console).
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    ///     Dropbox App Client Secret (App Secret from Dropbox App Console).
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;
}