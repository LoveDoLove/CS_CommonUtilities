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

namespace CommonUtilities.Helpers.MegaDrive;

public interface IMegaDriveHelper
{
    /// <summary>
    ///     Logs out the client.
    /// </summary>
    Task LogoutAsync();

    /// <summary>
    ///     Lists files and folders under a parent node. Returns file info objects.
    /// </summary>
    /// <param name="parentId">Optional parent node ID. If null, uses root.</param>
    /// <returns>List of file/folder info objects.</returns>
    Task<IList<MegaNodeInfo>> ListFilesAsync(string? parentId = null);

    /// <summary>
    ///     Uploads a file to Mega Drive. Returns upload result info.
    /// </summary>
    /// <param name="filePath">Local file path to upload.</param>
    /// <param name="parentId">Optional parent node ID. If null, uploads to root.</param>
    /// <returns>Upload result info including download link.</returns>
    Task<MegaUploadResult> UploadFileAsync(string filePath, string? parentId = null);

    /// <summary>
    ///     Downloads a file from Mega Drive. Returns local path and status.
    /// </summary>
    /// <param name="fileId">Node ID of the file to download.</param>
    /// <param name="destinationPath">Local destination path.</param>
    /// <returns>Download result info.</returns>
    Task<MegaDownloadResult> DownloadFileAsync(string fileId, string destinationPath);

    /// <summary>
    ///     Renames a file or folder.
    /// </summary>
    /// <param name="nodeId">Node ID to rename.</param>
    /// <param name="newName">New name.</param>
    /// <returns>True if successful.</returns>
    Task<bool> RenameNodeAsync(string nodeId, string newName);

    /// <summary>
    ///     Moves a file or folder to a new parent.
    /// </summary>
    /// <param name="nodeId">Node ID to move.</param>
    /// <param name="newParentId">New parent node ID.</param>
    /// <returns>True if successful.</returns>
    Task<bool> MoveNodeAsync(string nodeId, string newParentId);

    /// <summary>
    ///     Deletes a file or folder.
    /// </summary>
    /// <param name="nodeId">Node ID to delete.</param>
    /// <returns>True if successful.</returns>
    Task<bool> DeleteNodeAsync(string nodeId);
}