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

using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Helpers.Media;

public interface IImageDropboxHelper
{
    /// <summary>
    ///     Uploads and resizes an image to Dropbox.
    /// </summary>
    /// <param name="f">The uploaded form file.</param>
    /// <param name="dropboxPath">Target Dropbox path (e.g., "/images/image.jpg").</param>
    /// <returns>Metadata of the uploaded file.</returns>
    Task<FileMetadata> UploadPhotoToDropboxAsync(IFormFile f, string dropboxPath);

    /// <summary>
    ///     Downloads an image from Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>Stream of the downloaded image.</returns>
    Task<Stream> DownloadPhotoFromDropboxAsync(string dropboxPath);

    /// <summary>
    ///     Updates (re-uploads) an image to Dropbox.
    /// </summary>
    /// <param name="f">The new form file.</param>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>Metadata of the updated file.</returns>
    Task<FileMetadata> UpdatePhotoInDropboxAsync(IFormFile f, string dropboxPath);

    /// <summary>
    ///     Deletes an image from Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>Metadata of the deleted file.</returns>
    Task<DeleteResult> DeletePhotoFromDropboxAsync(string dropboxPath);

    /// <summary>
    ///     Gets a direct preview link for an image in Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>URL for direct image preview.</returns>
    Task<string> GetDropboxImagePreviewLinkAsync(string dropboxPath);
}