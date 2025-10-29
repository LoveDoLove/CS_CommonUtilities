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
    ///     Access Token (short-lived)
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    ///     Refresh Token (long-lived)
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    ///     Dropbox App Client ID
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    ///     Dropbox App Client Secret
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;
}