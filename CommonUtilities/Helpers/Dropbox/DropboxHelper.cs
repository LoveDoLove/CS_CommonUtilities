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

using System.Text.Json;
using CommonUtilities.Utilities.System;
using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using Dropbox.Api.Stone;

namespace CommonUtilities.Helpers.Dropbox;

/// <summary>
///     Helper class for Dropbox CRUD operations and direct image preview link retrieval.
///     Dropbox OAuth2 Refresh Token Flow:
///     - You must obtain a refresh token via the OAuth2 authorization flow with token_access_type=offline.
///     - The App Console 'Generate' button does NOT provide a refresh token.
///     - Manual steps:
///     1. Visit https://www.dropbox.com/oauth2/authorize?client_id=APPKEYHERE&response_type=code&token_access_type=offline
///     2. Authorize the app and copy the authorization code from the redirect.
///     3. Exchange the code for a refresh token using:
///     curl https://api.dropbox.com/oauth2/token -d code=AUTHORIZATIONCODEHERE -d grant_type=authorization_code -u
///     APPKEYHERE:APPSECRETHERE
///     4. Store the returned refresh token in configuration.
///     See Dropbox OAuth guide: https://developers.dropbox.com/oauth-guide
/// </summary>
public class DropboxHelper : IDropboxHelper
{
    private const string AccessTokenCacheKey = "DropboxAccessToken";

    private static readonly TimeSpan
        AccessTokenCacheDuration = TimeSpan.FromMinutes(200); // Dropbox tokens expire in 4h, use 200min for safety

    private readonly DropboxConfig _config;
    private DropboxClient _client;

    public DropboxHelper(DropboxConfig config)
    {
        _config = config;
        string accessToken = GetOrRefreshAccessToken();
        _client = new DropboxClient(accessToken);
    }

    /// <summary>
    ///     Uploads a file to Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Target Dropbox path (e.g., "/folder/image.jpg").</param>
    /// <param name="fileStream">Stream of the file to upload.</param>
    /// <returns>Metadata of the uploaded file.</returns>
    public async Task<FileMetadata> CreateAsync(string dropboxPath, Stream fileStream)
    {
        EnsureClient();
        FileMetadata? result =
            await _client.Files.UploadAsync(dropboxPath, WriteMode.Overwrite.Instance, body: fileStream);
        return result;
    }

    /// <summary>
    ///     Downloads a file from Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>Stream of the downloaded file.</returns>
    public async Task<Stream> ReadAsync(string dropboxPath)
    {
        EnsureClient();
        IDownloadResponse<FileMetadata>? response = await _client.Files.DownloadAsync(dropboxPath);
        return await response.GetContentAsStreamAsync();
    }

    /// <summary>
    ///     Updates a file in Dropbox (re-upload).
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <param name="fileStream">Stream of the new file.</param>
    /// <returns>Metadata of the updated file.</returns>
    public async Task<FileMetadata> UpdateAsync(string dropboxPath, Stream fileStream)
    {
        EnsureClient();
        FileMetadata? result =
            await _client.Files.UploadAsync(dropboxPath, WriteMode.Overwrite.Instance, body: fileStream);
        return result;
    }

    /// <summary>
    ///     Moves or renames a file in Dropbox.
    /// </summary>
    /// <param name="fromPath">Current Dropbox file path.</param>
    /// <param name="toPath">New Dropbox file path.</param>
    /// <returns>Metadata of the moved file.</returns>
    public async Task<FileMetadata> MoveAsync(string fromPath, string toPath)
    {
        EnsureClient();
        RelocationResult? result = await _client.Files.MoveV2Async(fromPath, toPath, autorename: false);
        return result.Metadata.AsFile;
    }

    /// <summary>
    ///     Deletes a file from Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>Metadata of the deleted file.</returns>
    public async Task<DeleteResult> DeleteAsync(string dropboxPath)
    {
        EnsureClient();
        DeleteResult? result = await _client.Files.DeleteV2Async(dropboxPath);
        return result;
    }

    /// <summary>
    ///     Gets a direct preview link for an image file in Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>URL for previewing the image.</returns>
    public async Task<string> GetPreviewLinkAsync(string dropboxPath)
    {
        EnsureClient();
        SharedLinkMetadata? sharedLinkResult = await _client.Sharing.CreateSharedLinkWithSettingsAsync(dropboxPath);
        // Dropbox shared links can be modified for direct image preview
        // Replace ?dl=0 with ?raw=1 for direct image rendering
        return sharedLinkResult.Url.Replace("dl=0", "raw=1");
    }

    /// <summary>
    ///     Gets the current access token from cache, or refreshes it using the refresh token if expired.
    /// </summary>
    private string GetOrRefreshAccessToken()
    {
        // Try to get from cache
        string cachedToken = CacheUtilities.GetOrAdd(
            AccessTokenCacheKey,
            RefreshAccessToken,
            AccessTokenCacheDuration
        );
        return cachedToken;
    }

    /// <summary>
    ///     Refreshes the Dropbox access token using the refresh token and updates config.
    /// </summary>
    private string RefreshAccessToken()
    {
        // Use Dropbox OAuth2 token endpoint to refresh access token.
        // This requires a valid refresh token obtained via the OAuth2 authorization flow.
        using HttpClient client = new();
        Dictionary<string, string> values = new()
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", _config.RefreshToken },
            { "client_id", _config.ClientId },
            { "client_secret", _config.ClientSecret }
        };
        FormUrlEncodedContent content = new(values);
        HttpResponseMessage response = client.PostAsync("https://api.dropbox.com/oauth2/token", content).Result;
        string json = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
            // If you get 'invalid_grant' or 'refresh token is malformed', the refresh token is invalid or not generated via the correct flow.
            throw new InvalidOperationException($"Dropbox token refresh failed: {response.StatusCode} {json}");
        JsonDocument tokenObj = JsonDocument.Parse(json);
        string? accessToken = tokenObj.RootElement.GetProperty("access_token").GetString();
        // Optionally update config
        _config.AccessToken = accessToken ?? string.Empty;
        return accessToken ?? string.Empty;
    }

    /// <summary>
    ///     Ensures the DropboxClient is initialized with a valid access token.
    /// </summary>
    private void EnsureClient()
    {
        string accessToken = GetOrRefreshAccessToken();
        // DropboxClient does not expose AccessToken property, so always re-instantiate for safety
        _client = new DropboxClient(accessToken);
    }
}