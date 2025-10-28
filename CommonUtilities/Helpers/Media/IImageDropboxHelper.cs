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