using System.Text.RegularExpressions;
using CommonUtilities.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using File = System.IO.File;

namespace CommonUtilities.Services.Media;

/// <summary>
///     Service for handling image uploads, validation, resizing, and deletion.
/// </summary>
public class ImageService : IImageService
{
    private const string ImageType = @"^image\/(jpeg|png)$";
    private const string ImageName = @"^.+\.(jpeg|jpg|png)$";
    private const int ImageSize = 5 * 1024 * 1024; // 5MB

    private static readonly Regex ImageTypeRegex = new(ImageType, RegexOptions.IgnoreCase);
    private static readonly Regex ImageNameRegex = new(ImageName, RegexOptions.IgnoreCase);

    private readonly IWebHostEnvironment _environment;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ImageService" /> class.
    /// </summary>
    /// <param name="environment">The web host environment, used to determine file paths.</param>
    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    /// <summary>
    ///     Validates an uploaded photo based on type (JPEG/PNG) and size (max 5MB).
    /// </summary>
    /// <param name="f">The IFormFile representing the uploaded photo.</param>
    /// <returns>An empty string if the photo is valid, otherwise an error message.</returns>
    public string ValidatePhoto(IFormFile f)
    {
        if (!ImageTypeRegex.IsMatch(f.ContentType) || !ImageNameRegex.IsMatch(f.FileName))
            return "Only JPG and PNG photo is allowed.";

        return f.Length > ImageSize ? "Photo size cannot more than 5MB." : "";
    }

    /// <summary>
    ///     Saves an uploaded photo to the specified folder after resizing it.
    ///     The photo is resized to 200x200 pixels using a crop mode.
    ///     The saved filename is a GUID.
    /// </summary>
    /// <param name="f">The IFormFile representing the uploaded photo.</param>
    /// <param name="folder">The subfolder within the web root's 'wwwroot' directory to save the photo.</param>
    /// <returns>The filename of the saved photo.</returns>
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
    ///     Deletes a photo from the specified folder.
    /// </summary>
    /// <param name="file">The filename of the photo to delete.</param>
    /// <param name="folder">The subfolder within the web root's 'wwwroot' directory where the photo is located.</param>
    public void DeletePhoto(string file, string folder)
    {
        file = Path.GetFileName(file);
        string path = Path.Combine(_environment.WebRootPath, folder, file);
        File.Delete(path);
    }
}