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

using CommonUtilities.Helpers.Dropbox;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace CommonUtilities.Helpers.Media;

/// <summary>
///     Extends ImageHelper to support Dropbox image CRUD operations.
/// </summary>
public class ImageDropboxHelper : IImageDropboxHelper
{
    private readonly IDropboxHelper _dropboxHelper;

    /// <summary>
    ///     Initializes a new instance of ImageDropboxHelper with the specified environment and DropboxHelper.
    /// </summary>
    /// <param name="dropboxHelper">DropboxHelper instance for Dropbox operations.</param>
    public ImageDropboxHelper(IDropboxHelper dropboxHelper)
    {
        _dropboxHelper = dropboxHelper;
    }

    /// <summary>
    ///     Uploads and resizes an image to Dropbox.
    /// </summary>
    /// <param name="f">The uploaded form file.</param>
    /// <param name="dropboxPath">Target Dropbox path (e.g., "/images/image.jpg").</param>
    /// <returns>Metadata of the uploaded file.</returns>
    public async Task<FileMetadata> UploadPhotoToDropboxAsync(IFormFile f, string dropboxPath)
    {
        await using Stream inputStream = f.OpenReadStream();
        using Image image = await Image.LoadAsync(inputStream);
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(200, 200),
            Mode = ResizeMode.Crop
        }));
        using MemoryStream outputStream = new MemoryStream();
        string ext = Path.GetExtension(dropboxPath).ToLowerInvariant();
        if (ext == ".png")
            await image.SaveAsync(outputStream, PngFormat.Instance);
        else
            await image.SaveAsync(outputStream, JpegFormat.Instance);
        outputStream.Position = 0;
        return await _dropboxHelper.CreateAsync(dropboxPath, outputStream);
    }

    /// <summary>
    ///     Downloads an image from Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>Stream of the downloaded image.</returns>
    public async Task<Stream> DownloadPhotoFromDropboxAsync(string dropboxPath)
    {
        return await _dropboxHelper.ReadAsync(dropboxPath);
    }

    /// <summary>
    ///     Updates (re-uploads) an image to Dropbox.
    /// </summary>
    /// <param name="f">The new form file.</param>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>Metadata of the updated file.</returns>
    public async Task<FileMetadata> UpdatePhotoInDropboxAsync(IFormFile f, string dropboxPath)
    {
        await using Stream inputStream = f.OpenReadStream();
        using Image image = await Image.LoadAsync(inputStream);
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(200, 200),
            Mode = ResizeMode.Crop
        }));
        using MemoryStream outputStream = new MemoryStream();
        string ext = Path.GetExtension(dropboxPath).ToLowerInvariant();
        if (ext == ".png")
            await image.SaveAsync(outputStream, PngFormat.Instance);
        else
            await image.SaveAsync(outputStream, JpegFormat.Instance);
        outputStream.Position = 0;
        return await _dropboxHelper.UpdateAsync(dropboxPath, outputStream);
    }

    /// <summary>
    ///     Deletes an image from Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>Metadata of the deleted file.</returns>
    public async Task<DeleteResult> DeletePhotoFromDropboxAsync(string dropboxPath)
    {
        return await _dropboxHelper.DeleteAsync(dropboxPath);
    }

    /// <summary>
    ///     Gets a direct preview link for an image in Dropbox.
    /// </summary>
    /// <param name="dropboxPath">Dropbox file path.</param>
    /// <returns>URL for direct image preview.</returns>
    public async Task<string> GetDropboxImagePreviewLinkAsync(string dropboxPath)
    {
        return await _dropboxHelper.GetPreviewLinkAsync(dropboxPath);
    }
}