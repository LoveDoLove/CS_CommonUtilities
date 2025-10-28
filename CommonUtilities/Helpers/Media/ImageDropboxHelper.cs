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

using CommonUtilities.Helpers.Dropbox;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace CommonUtilities.Helpers.Media;

/// <summary>
///     Extends ImageHelper to support Dropbox image CRUD operations.
/// </summary>
public class ImageDropboxHelper : ImageHelper
{
    private readonly DropboxHelper _dropboxHelper;

    /// <summary>
    ///     Initializes a new instance of ImageDropboxHelper with the specified environment and DropboxHelper.
    /// </summary>
    /// <param name="environment">Web host environment for local file operations.</param>
    /// <param name="dropboxHelper">DropboxHelper instance for Dropbox operations.</param>
    public ImageDropboxHelper(IWebHostEnvironment environment, DropboxHelper dropboxHelper)
        : base(environment)
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