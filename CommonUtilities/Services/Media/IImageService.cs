using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Services.Media;

/// <summary>
///     Defines the contract for a service that handles image processing,
///     including validation, saving, and deletion of photos.
/// </summary>
public interface IImageService
{
    /// <summary>
    ///     Validates an uploaded photo file based on predefined criteria (e.g., file type, size).
    /// </summary>
    /// <param name="f">The <see cref="IFormFile" /> representing the uploaded photo.</param>
    /// <returns>
    ///     An empty string if the photo is valid; otherwise, an error message indicating the reason for invalidity.
    /// </returns>
    string ValidatePhoto(IFormFile f);

    /// <summary>
    ///     Saves an uploaded photo file to a specified folder.
    /// </summary>
    /// <param name="f">The <see cref="IFormFile" /> representing the uploaded photo to save.</param>
    /// <param name="folder">
    ///     The name of the folder (or subpath) where the photo should be saved.
    ///     This is typically relative to a preconfigured base storage path.
    /// </param>
    /// <returns>
    ///     The unique filename (or path relative to the base storage) under which the photo was saved if successful;
    ///     otherwise, an empty string or an error indicator if saving failed.
    /// </returns>
    string SavePhoto(IFormFile f, string folder);

    /// <summary>
    ///     Deletes a photo file from a specified folder.
    /// </summary>
    /// <param name="file">The name of the file to delete.</param>
    /// <param name="folder">
    ///     The name of the folder (or subpath) where the photo is located.
    ///     This is typically relative to a preconfigured base storage path.
    /// </param>
    void DeletePhoto(string file, string folder);
}