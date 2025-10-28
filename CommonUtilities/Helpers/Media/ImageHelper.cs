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

using System.Text.RegularExpressions;
using CG.Web.MegaApiClient;
using CommonUtilities.Helpers.MegaDrive;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using File = System.IO.File;

namespace CommonUtilities.Helpers.Media;

/// <summary>
///     Provides helper methods for validating, saving, resizing, and deleting image files in the web application.
/// </summary>
public class ImageHelper : IImageHelper
{
    private const string ImageType = @"^image\/(jpeg|png)$";
    private const string ImageName = @"^.+\.(jpeg|jpg|png)$";
    private const int ImageSize = 5 * 1024 * 1024; // 5MB

    private static readonly Regex ImageTypeRegex = new(ImageType, RegexOptions.IgnoreCase);
    private static readonly Regex ImageNameRegex = new(ImageName, RegexOptions.IgnoreCase);

    private readonly IWebHostEnvironment _environment;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ImageHelper" /> class with the specified web host environment.
    /// </summary>
    /// <param name="environment">The web host environment for file storage.</param>
    public ImageHelper(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    /// <summary>
    ///     Validates the uploaded photo file for type and size constraints.
    /// </summary>
    /// <param name="f">The uploaded form file to validate.</param>
    /// <returns>An error message if validation fails; otherwise, an empty string.</returns>
    public string ValidatePhoto(IFormFile f)
    {
        if (!ImageTypeRegex.IsMatch(f.ContentType) || !ImageNameRegex.IsMatch(f.FileName))
            return "Only JPG and PNG photo is allowed.";

        return f.Length > ImageSize ? "Photo size cannot more than 5MB." : "";
    }

    /// <summary>
    ///     Saves the uploaded photo file to the specified folder, resizing it as needed.
    /// </summary>
    /// <param name="f">The uploaded form file to save.</param>
    /// <param name="folder">The target folder for saving the image.</param>
    /// <returns>The saved file name.</returns>
    public string SavePhoto(IFormFile f, string folder)
    {
        string originalExtension = Path.GetExtension(f.FileName).ToLowerInvariant();
        string targetExtension = ".jpg"; // Default to jpg

        // Allow specific extensions or preserve original if it's one of the allowed types
        if (originalExtension == ".png" || originalExtension == ".jpeg" || originalExtension == ".jpg")
            targetExtension = originalExtension;
        // Ensure the target extension is one that SixLabors.ImageSharp can save easily by default.
        // Forcing to .jpg if unsure is a safe bet for broad compatibility if original format isn't critical.
        // If original format (like PNG transparency) is critical, more sophisticated handling is needed.
        // For this example, we'll use .jpg if it's not png or jpeg/jpg.
        if (targetExtension != ".png" && targetExtension != ".jpeg" && targetExtension != ".jpg")
            targetExtension = ".jpg";


        string fileName = Guid.NewGuid().ToString("n") + targetExtension;
        string path = Path.Combine(_environment.WebRootPath, folder, fileName);

        ResizeOptions options = new()
        {
            Size = new Size(200, 200),
            Mode = ResizeMode.Crop
        };

        using Stream stream = f.OpenReadStream();
        using Image img = Image.Load(stream);
        img.Mutate(x => x.Resize(options));
        img.Save(path); // ImageSharp will attempt to save in the format indicated by the path's extension

        return fileName;
    }

    /// <summary>
    ///     Deletes the specified photo file from the given folder.
    /// </summary>
    /// <param name="file">The file name to delete.</param>
    /// <param name="folder">The folder containing the file.</param>
    public void DeletePhoto(string file, string folder)
    {
        file = Path.GetFileName(file);
        string path = Path.Combine(_environment.WebRootPath, folder, file);
        File.Delete(path);
    }

    /// <summary>
    ///     Saves the uploaded photo file to Mega Drive, resizing it as needed.
    /// </summary>
    /// <param name="f">The uploaded form file to save.</param>
    /// <param name="megaDriveHelper">An instance of IMegaDriveHelper for uploading.</param>
    /// <returns>The MegaDrive upload result.</returns>
    public async Task<MegaUploadResult> SavePhotoToMegaDriveAsync(IFormFile f, IMegaDriveHelper megaDriveHelper)
    {
        string originalExtension = Path.GetExtension(f.FileName).ToLowerInvariant();
        string targetExtension = ".jpg";
        if (originalExtension == ".png" || originalExtension == ".jpeg" || originalExtension == ".jpg")
            targetExtension = originalExtension;
        if (targetExtension != ".png" && targetExtension != ".jpeg" && targetExtension != ".jpg")
            targetExtension = ".jpg";

        ResizeOptions options = new()
        {
            Size = new Size(200, 200),
            Mode = ResizeMode.Crop
        };

        await using Stream inputStream = f.OpenReadStream();
        using Image img = await Image.LoadAsync(inputStream);
        img.Mutate(x => x.Resize(options));
        using MemoryStream outputStream = new();
        if (targetExtension == ".png")
            await img.SaveAsync(outputStream, PngFormat.Instance);
        else
            await img.SaveAsync(outputStream, JpegFormat.Instance);
        outputStream.Position = 0;

        // Save to temp file for upload
        string tempFile = Path.GetTempFileName() + targetExtension;
        await using (FileStream fs = new(tempFile, FileMode.Create, FileAccess.Write))
        {
            await outputStream.CopyToAsync(fs);
        }

        MegaUploadResult result = await megaDriveHelper.UploadFileAsync(tempFile);
        File.Delete(tempFile);
        return result;
    }

    /// <summary>
    ///     Removes a photo from Mega Drive by file ID.
    /// </summary>
    /// <param name="fileId">The Mega Drive file ID to remove.</param>
    /// <param name="megaDriveHelper">An instance of IMegaDriveHelper for deletion.</param>
    /// <returns>True if successful.</returns>
    public async Task<bool> RemovePhotoFromMegaDriveAsync(string fileId, IMegaDriveHelper megaDriveHelper)
    {
        return await megaDriveHelper.DeleteNodeAsync(fileId);
    }

    /// <summary>
    ///     Gets a photo from Mega Drive by file ID.
    /// </summary>
    /// <param name="fileId">The Mega Drive file ID to retrieve.</param>
    /// <param name="destinationPath">Local destination path.</param>
    /// <param name="megaDriveHelper">An instance of IMegaDriveHelper for download.</param>
    /// <returns>Download result info.</returns>
    public async Task<MegaDownloadResult> GetPhotoFromMegaDriveAsync(string fileId, string destinationPath,
        IMegaDriveHelper megaDriveHelper)
    {
        return await megaDriveHelper.DownloadFileAsync(fileId, destinationPath);
    }

    /// <summary>
    ///     Downloads and caches an image from a public Mega.nz URL.
    ///     This method handles downloading, caching, and MIME type detection for Mega.nz public image links.
    /// </summary>
    /// <param name="megaPublicUrl">The public Mega.nz URL (e.g., https://mega.nz/file/xxx#yyy).</param>
    /// <param name="cacheDirectory">The directory where cached images should be stored.</param>
    /// <param name="filePrefix">Optional prefix for the cached filename (default: "cached").</param>
    /// <returns>Result containing the cached file path, MIME type, and success status.</returns>
    public async Task<MegaImageCacheResult> DownloadAndCacheMegaImageAsync(string megaPublicUrl,
        string cacheDirectory, string filePrefix = "cached")
    {
        // Validate input
        if (string.IsNullOrEmpty(megaPublicUrl))
            return new MegaImageCacheResult
            {
                Success = false,
                ErrorMessage = "Image URL is required"
            };

        if (!megaPublicUrl.StartsWith("https://mega.nz/", StringComparison.OrdinalIgnoreCase))
            return new MegaImageCacheResult
            {
                Success = false,
                ErrorMessage = "Only Mega.nz URLs are supported"
            };

        try
        {
            // Create cache directory if it doesn't exist
            if (!Directory.Exists(cacheDirectory)) Directory.CreateDirectory(cacheDirectory);

            // Use hash for cache filename (without extension initially)
            string fileHash = $"{filePrefix}_{Math.Abs(megaPublicUrl.GetHashCode())}";

            // Check if any cached file with this hash exists (with any extension)
            string[] cachedFiles = Directory.GetFiles(cacheDirectory, $"{fileHash}.*");
            if (cachedFiles.Length > 0)
            {
                string cachedFile = cachedFiles[0];
                string mimeType = GetMimeTypeFromExtension(Path.GetExtension(cachedFile));
                return new MegaImageCacheResult
                {
                    Success = true,
                    CachedFilePath = cachedFile,
                    MimeType = mimeType
                };
            }

            // Download from Mega.nz using MegaApiClient for public URLs
            MegaApiClient megaClient = new();
            await megaClient.LoginAnonymousAsync();

            try
            {
                // Get file info from public URL to determine the actual filename
                Uri megaUri = new(megaPublicUrl);
                INode? nodeInfo = await megaClient.GetNodeFromLinkAsync(megaUri);

                if (nodeInfo == null)
                    return new MegaImageCacheResult
                    {
                        Success = false,
                        ErrorMessage = "Unable to retrieve file information from Mega.nz"
                    };

                // Get actual file extension from the node name
                string actualExtension = Path.GetExtension(nodeInfo.Name);
                if (string.IsNullOrEmpty(actualExtension)) actualExtension = ".jpg"; // Default fallback

                string cachePath = Path.Combine(cacheDirectory, $"{fileHash}{actualExtension}");

                // Download the file
                await megaClient.DownloadFileAsync(megaUri, cachePath);

                // Verify the file was downloaded
                if (!File.Exists(cachePath))
                    return new MegaImageCacheResult
                    {
                        Success = false,
                        ErrorMessage = "Failed to download image from Mega.nz"
                    };

                string mimeType = GetMimeTypeFromExtension(actualExtension);
                return new MegaImageCacheResult
                {
                    Success = true,
                    CachedFilePath = cachePath,
                    MimeType = mimeType
                };
            }
            finally
            {
                await megaClient.LogoutAsync();
            }
        }
        catch (Exception ex)
        {
            return new MegaImageCacheResult
            {
                Success = false,
                ErrorMessage = $"Failed to download image: {ex.Message}"
            };
        }
    }

    /// <summary>
    ///     Gets the MIME type for an image file based on its extension.
    /// </summary>
    /// <param name="extension">The file extension (with or without leading dot).</param>
    /// <returns>The MIME type string.</returns>
    public string GetMimeTypeFromExtension(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".svg" => "image/svg+xml",
            _ => "image/jpeg" // Default fallback
        };
    }
}