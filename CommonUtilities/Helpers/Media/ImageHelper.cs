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
using CommonUtilities.Helpers.GoogleDrive;
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
    ///     Saves the uploaded photo file to Google Drive, resizing it as needed.
    /// </summary>
    /// <param name="f"></param>
    /// <param name="driveHelper"></param>
    /// <returns></returns>
    public string SavePhotoToGoogleDrive(IFormFile f, IGoogleDriveHelper driveHelper)
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

        using Stream inputStream = f.OpenReadStream();
        using Image img = Image.Load(inputStream);
        img.Mutate(x => x.Resize(options));
        using MemoryStream outputStream = new MemoryStream();
        if (targetExtension == ".png")
            img.Save(outputStream, PngFormat.Instance);
        else
            img.Save(outputStream, JpegFormat.Instance);
        outputStream.Position = 0;

        var driveFile = driveHelper.CreateFile(f.FileName, f.ContentType, outputStream);
        return driveFile.Id;
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
}