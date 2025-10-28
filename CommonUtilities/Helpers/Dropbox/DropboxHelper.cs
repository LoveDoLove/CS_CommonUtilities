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

using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using Dropbox.Api.Stone;

namespace CommonUtilities.Helpers.Dropbox;

/// <summary>
///     Helper class for Dropbox CRUD operations and direct image preview link retrieval.
/// </summary>
public class DropboxHelper : IDropboxHelper
{
    private readonly DropboxClient _client;
    private readonly DropboxConfig _config;

    /// <summary>
    ///     Initializes a new instance of DropboxHelper with the given access token.
    /// </summary>
    /// <param name="config"></param>
    public DropboxHelper(DropboxConfig config)
    {
        _config = config;
        _client = new DropboxClient(config.AccessToken);
    }

    /// <summary>
    ///     Uploads a file to Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Target Dropbox path (e.g., "/folder/image.jpg").</param>
    /// <param name="fileStream">Stream of the file to upload.</param>
    /// <returns>Metadata of the uploaded file.</returns>
    public async Task<FileMetadata> CreateAsync(string dropboxPath, Stream fileStream)
    {
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
        // Overwrite is used for update
        FileMetadata? result =
            await _client.Files.UploadAsync(dropboxPath, WriteMode.Overwrite.Instance, body: fileStream);
        return result;
    }

    /// <summary>
    ///     Deletes a file from Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>Metadata of the deleted file.</returns>
    public async Task<DeleteResult> DeleteAsync(string dropboxPath)
    {
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
        SharedLinkMetadata? sharedLinkResult = await _client.Sharing.CreateSharedLinkWithSettingsAsync(dropboxPath);
        // Dropbox shared links can be modified for direct image preview
        // Replace ?dl=0 with ?raw=1 for direct image rendering
        return sharedLinkResult.Url.Replace("dl=0", "raw=1");
    }
}