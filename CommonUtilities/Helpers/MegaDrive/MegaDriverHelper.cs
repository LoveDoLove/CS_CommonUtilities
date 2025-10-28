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

using CG.Web.MegaApiClient;

namespace CommonUtilities.Helpers.MegaDrive;

/// <summary>
///     Helper class for Mega Drive CRUD operations, designed for easy integration in any project.
/// </summary>
public class MegaDriveHelper : IMegaDriveHelper
{
    private readonly MegaApiClient _megaApiClient;
    private readonly MegaDriveConfig _megaDriveConfig;
    private bool _isLoggedIn;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MegaDriveHelper" /> class.
    /// </summary>
    /// <param name="megaDriveConfig">Mega Drive configuration.</param>
    public MegaDriveHelper(MegaDriveConfig megaDriveConfig)
    {
        _megaDriveConfig = megaDriveConfig;
        _megaApiClient = new MegaApiClient();
    }

    /// <summary>
    ///     Logs out the client.
    /// </summary>
    public async Task LogoutAsync()
    {
        if (_isLoggedIn)
        {
            await _megaApiClient.LogoutAsync();
            _isLoggedIn = false;
        }
    }

    /// <summary>
    ///     Lists files and folders under a parent node. Returns file info objects.
    /// </summary>
    /// <param name="parentId">Optional parent node ID. If null, uses root.</param>
    /// <returns>List of file/folder info objects.</returns>
    public async Task<IList<MegaNodeInfo>> ListFilesAsync(string? parentId = null)
    {
        await EnsureLoggedInAsync();
        IEnumerable<INode> nodes = await _megaApiClient.GetNodesAsync();
        INode? parent = parentId == null
            ? nodes.Single(n => n.Type == NodeType.Root)
            : nodes.SingleOrDefault(n => n.Id == parentId);
        if (parent == null)
            throw new ArgumentException("Parent node not found.");

        List<MegaNodeInfo> result = new();
        foreach (INode node in nodes.Where(x => x.ParentId == parent.Id))
        {
            Uri? downloadLink = null;
            if (node.Type == NodeType.File)
                try
                {
                    downloadLink = await _megaApiClient.GetDownloadLinkAsync(node);
                }
                catch
                {
                    // ignored
                }

            result.Add(new MegaNodeInfo
            {
                Id = node.Id,
                Name = node.Name,
                Type = node.Type.ToString(),
                Size = node.Size,
                CreationDate = node.CreationDate,
                DownloadLink = downloadLink?.ToString()
            });
        }

        return result;
    }

    /// <summary>
    ///     Uploads a file to Mega Drive. Returns upload result info.
    /// </summary>
    /// <param name="filePath">Local file path to upload.</param>
    /// <param name="parentId">Optional parent node ID. If null, uploads to root.</param>
    /// <returns>Upload result info including download link.</returns>
    public async Task<MegaUploadResult> UploadFileAsync(string filePath, string? parentId = null)
    {
        await EnsureLoggedInAsync();
        IEnumerable<INode> nodes = await _megaApiClient.GetNodesAsync();
        INode? parent = parentId == null
            ? nodes.Single(n => n.Type == NodeType.Root)
            : nodes.SingleOrDefault(n => n.Id == parentId);
        if (parent == null)
            throw new ArgumentException("Parent node not found.");

        INode uploadedNode = await Task.Run(() => _megaApiClient.UploadFile(filePath, parent));
        Uri downloadLink = await _megaApiClient.GetDownloadLinkAsync(uploadedNode);
        return new MegaUploadResult
        {
            Id = uploadedNode.Id,
            Name = uploadedNode.Name,
            DownloadLink = downloadLink.ToString(),
            Size = uploadedNode.Size
        };
    }

    /// <summary>
    ///     Downloads a file from Mega Drive. Returns local path and status.
    /// </summary>
    /// <param name="fileId">Node ID of the file to download.</param>
    /// <param name="destinationPath">Local destination path.</param>
    /// <returns>Download result info.</returns>
    public async Task<MegaDownloadResult> DownloadFileAsync(string fileId, string destinationPath)
    {
        await EnsureLoggedInAsync();
        IEnumerable<INode> nodes = await _megaApiClient.GetNodesAsync();
        INode? fileNode = nodes.SingleOrDefault(n => n.Id == fileId && n.Type == NodeType.File);
        if (fileNode == null)
            return new MegaDownloadResult { Success = false, ErrorMessage = "File not found." };
        try
        {
            await Task.Run(() => _megaApiClient.DownloadFile(fileNode, destinationPath));
            return new MegaDownloadResult { Success = true, LocalPath = destinationPath };
        }
        catch (Exception ex)
        {
            return new MegaDownloadResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    /// <summary>
    ///     Renames a file or folder.
    /// </summary>
    /// <param name="nodeId">Node ID to rename.</param>
    /// <param name="newName">New name.</param>
    /// <returns>True if successful.</returns>
    public async Task<bool> RenameNodeAsync(string nodeId, string newName)
    {
        await EnsureLoggedInAsync();
        IEnumerable<INode?> nodes = await _megaApiClient.GetNodesAsync();
        INode? node = nodes.SingleOrDefault(n => n?.Id == nodeId);
        if (node == null) return false;
        try
        {
            await Task.Run(() => _megaApiClient.Rename(node, newName));
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     Moves a file or folder to a new parent.
    /// </summary>
    /// <param name="nodeId">Node ID to move.</param>
    /// <param name="newParentId">New parent node ID.</param>
    /// <returns>True if successful.</returns>
    public async Task<bool> MoveNodeAsync(string nodeId, string newParentId)
    {
        await EnsureLoggedInAsync();
        IEnumerable<INode?> nodes = await _megaApiClient.GetNodesAsync();
        INode? node = nodes.SingleOrDefault(n => n.Id == nodeId);
        INode newParent = nodes.SingleOrDefault(n => n?.Id == newParentId);
        if (node == null || newParent == null) return false;
        try
        {
            await Task.Run(() => _megaApiClient.Move(node, newParent));
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     Deletes a file or folder.
    /// </summary>
    /// <param name="nodeId">Node ID to delete.</param>
    /// <returns>True if successful.</returns>
    public async Task<bool> DeleteNodeAsync(string nodeId)
    {
        await EnsureLoggedInAsync();
        IEnumerable<INode?> nodes = await _megaApiClient.GetNodesAsync();
        INode? node = nodes.SingleOrDefault(n => n?.Id == nodeId);
        if (node == null) return false;
        try
        {
            await Task.Run(() => _megaApiClient.Delete(node));
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     Ensures the client is logged in before performing any operation.
    /// </summary>
    private async Task EnsureLoggedInAsync()
    {
        if (!_isLoggedIn)
        {
            await Task.Run(() => _megaApiClient.Login(_megaDriveConfig.Email, _megaDriveConfig.Password));
            _isLoggedIn = true;
        }
    }
}