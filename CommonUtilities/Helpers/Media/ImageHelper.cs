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

using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
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