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