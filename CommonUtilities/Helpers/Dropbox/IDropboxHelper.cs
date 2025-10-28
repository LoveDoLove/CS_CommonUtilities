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

namespace CommonUtilities.Helpers.Dropbox;

public interface IDropboxHelper
{
    /// <summary>
    ///     Uploads a file to Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Target Dropbox path (e.g., "/folder/image.jpg").</param>
    /// <param name="fileStream">Stream of the file to upload.</param>
    /// <returns>Metadata of the uploaded file.</returns>
    Task<FileMetadata> CreateAsync(string dropboxPath, Stream fileStream);

    /// <summary>
    ///     Downloads a file from Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>Stream of the downloaded file.</returns>
    Task<Stream> ReadAsync(string dropboxPath);

    /// <summary>
    ///     Updates a file in Dropbox (re-upload).
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <param name="fileStream">Stream of the new file.</param>
    /// <returns>Metadata of the updated file.</returns>
    Task<FileMetadata> UpdateAsync(string dropboxPath, Stream fileStream);

    /// <summary>
    ///     Moves or renames a file in Dropbox.
    /// </summary>
    /// <param name="fromPath">Current Dropbox file path.</param>
    /// <param name="toPath">New Dropbox file path.</param>
    /// <returns>Metadata of the moved file.</returns>
    Task<FileMetadata> MoveAsync(string fromPath, string toPath);

    /// <summary>
    ///     Deletes a file from Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>Metadata of the deleted file.</returns>
    Task<DeleteResult> DeleteAsync(string dropboxPath);

    /// <summary>
    ///     Gets a direct preview link for an image file in Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>URL for previewing the image.</returns>
    Task<string> GetPreviewLinkAsync(string dropboxPath);
}