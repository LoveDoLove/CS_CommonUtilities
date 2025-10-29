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
    ///     Gets the MIME type for an image file based on its extension.
    /// </summary>
    /// <param name="extension">The file extension (with or without leading dot).</param>
    /// <returns>The MIME type string.</returns>
    string GetMimeTypeFromExtension(string extension);
}