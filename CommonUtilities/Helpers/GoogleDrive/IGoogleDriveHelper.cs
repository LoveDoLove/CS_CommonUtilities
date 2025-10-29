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

using File = Google.Apis.Drive.v3.Data.File;

namespace CommonUtilities.Helpers.GoogleDrive;

public interface IGoogleDriveHelper
{
    /// <summary>
    ///     Lists files in the user's Drive.
    /// </summary>
    IList<File> ListFiles(int pageSize = 10);

    /// <summary>
    ///     Creates a file in Google Drive.
    /// </summary>
    File CreateFile(string name, string mimeType, Stream content);

    /// <summary>
    ///     Gets file metadata by ID.
    /// </summary>
    File GetFile(string fileId);

    /// <summary>
    ///     Updates a file's metadata or content.
    /// </summary>
    File UpdateFile(string fileId, string? newName = null, Stream? newContent = null, string? newMimeType = null);

    /// <summary>
    ///     Deletes a file by ID.
    /// </summary>
    void DeleteFile(string fileId);
}