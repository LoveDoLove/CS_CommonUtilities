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

using CommonUtilities.Helpers.MegaDrive;
using Microsoft.AspNetCore.Http;

namespace CommonUtilities.Helpers.Media;

public interface IImageHelper
{
    /// <summary>
    ///     Validates the uploaded photo file for type and size constraints.
    /// </summary>
    /// <param name="f">The uploaded form file to validate.</param>
    /// <returns>An error message if validation fails; otherwise, an empty string.</returns>
    string ValidatePhoto(IFormFile f);

    /// <summary>
    ///     Saves the uploaded photo file to the specified folder, resizing it as needed.
    /// </summary>
    /// <param name="f">The uploaded form file to save.</param>
    /// <param name="folder">The target folder for saving the image.</param>
    /// <returns>The saved file name.</returns>
    string SavePhoto(IFormFile f, string folder);

    /// <summary>
    ///     Deletes the specified photo file from the given folder.
    /// </summary>
    /// <param name="file">The file name to delete.</param>
    /// <param name="folder">The folder containing the file.</param>
    void DeletePhoto(string file, string folder);

    /// <summary>
    ///     Saves the uploaded photo file to Mega Drive, resizing it as needed.
    /// </summary>
    /// <param name="f">The uploaded form file to save.</param>
    /// <param name="megaDriveHelper">An instance of IMegaDriveHelper for uploading.</param>
    /// <returns>The MegaDrive upload result.</returns>
    Task<MegaUploadResult> SavePhotoToMegaDriveAsync(IFormFile f, IMegaDriveHelper megaDriveHelper);

    /// <summary>
    ///     Removes a photo from Mega Drive by file ID.
    /// </summary>
    /// <param name="fileId">The Mega Drive file ID to remove.</param>
    /// <param name="megaDriveHelper">An instance of IMegaDriveHelper for deletion.</param>
    /// <returns>True if successful.</returns>
    Task<bool> RemovePhotoFromMegaDriveAsync(string fileId, IMegaDriveHelper megaDriveHelper);

    /// <summary>
    ///     Gets a photo from Mega Drive by file ID.
    /// </summary>
    /// <param name="fileId">The Mega Drive file ID to retrieve.</param>
    /// <param name="destinationPath">Local destination path.</param>
    /// <param name="megaDriveHelper">An instance of IMegaDriveHelper for download.</param>
    /// <returns>Download result info.</returns>
    Task<MegaDownloadResult> GetPhotoFromMegaDriveAsync(string fileId, string destinationPath,
        IMegaDriveHelper megaDriveHelper);
}