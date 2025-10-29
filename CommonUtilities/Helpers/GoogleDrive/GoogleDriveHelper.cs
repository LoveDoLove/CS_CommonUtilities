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

using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using File = Google.Apis.Drive.v3.Data.File;

namespace CommonUtilities.Helpers.GoogleDrive;

/// <summary>
///     Helper class for Google Drive CRUD operations.
/// </summary>
public class GoogleDriveHelper : IGoogleDriveHelper
{
    private readonly DriveService _driveService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GoogleDriveHelper" /> class using API Key authentication.
    /// </summary>
    /// <param name="googleDriveConfig">Configuration containing the API Key.</param>
    public GoogleDriveHelper(GoogleDriveConfig googleDriveConfig)
    {
        // Initialize DriveService with API Key authentication.
        _driveService = new DriveService(new BaseClientService.Initializer
        {
            ApiKey = googleDriveConfig.ApiKey,
            ApplicationName = "GoogleDriveHelper"
        });
    }

    /// <summary>
    ///     Lists files in the user's Drive.
    /// </summary>
    public IList<File> ListFiles(int pageSize = 10)
    {
        FilesResource.ListRequest? request = _driveService.Files.List();
        request.PageSize = pageSize;
        request.Fields = "files(id, name)";
        FileList? result = request.Execute();
        return result.Files;
    }

    /// <summary>
    ///     Creates a file in Google Drive.
    /// </summary>
    public File CreateFile(string name, string mimeType, Stream content)
    {
        File fileMetadata = new File { Name = name };
        FilesResource.CreateMediaUpload? request = _driveService.Files.Create(fileMetadata, content, mimeType);
        request.Fields = "id";
        if (request.Upload().Status == UploadStatus.Completed && request.ResponseBody != null)
            return request.ResponseBody;
        throw new InvalidOperationException("File upload did not complete successfully or response body is null.");
    }

    /// <summary>
    ///     Gets file metadata by ID.
    /// </summary>
    public File GetFile(string fileId)
    {
        FilesResource.GetRequest? request = _driveService.Files.Get(fileId);
        request.Fields = "id, name, mimeType, size";
        return request.Execute();
    }

    /// <summary>
    ///     Updates a file's metadata or content.
    /// </summary>
    public File UpdateFile(string fileId, string? newName = null, Stream? newContent = null, string? newMimeType = null)
    {
        File fileMetadata = new File();
        if (!string.IsNullOrEmpty(newName)) fileMetadata.Name = newName;
        FilesResource.UpdateMediaUpload? request =
            _driveService.Files.Update(fileMetadata, fileId, newContent, newMimeType);
        request.Fields = "id, name";
        if (request.Upload().Status == UploadStatus.Completed && request.ResponseBody != null)
            return request.ResponseBody;
        throw new InvalidOperationException("File update did not complete successfully or response body is null.");
    }

    /// <summary>
    ///     Deletes a file by ID.
    /// </summary>
    public void DeleteFile(string fileId)
    {
        FilesResource.DeleteRequest? request = _driveService.Files.Delete(fileId);
        request.Execute();
    }
}