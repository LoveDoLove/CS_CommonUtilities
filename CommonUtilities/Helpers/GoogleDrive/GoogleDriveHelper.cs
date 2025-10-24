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

using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v3.Data.File;

namespace CommonUtilities.Helpers.GoogleDrive;

/// <summary>
///     Helper class for Google Drive CRUD operations.
/// </summary>
public class GoogleDriveHelper
{
    private readonly DriveService _driveService;

    /// <summary>
    ///     Initializes the Google Drive service using OAuth2 credentials.
    /// </summary>
    /// <param name="credentialsPath">Path to client_secret.json.</param>
    /// <param name="tokenPath">Path to store user token.</param>
    public GoogleDriveHelper(string credentialsPath, string tokenPath)
    {
        UserCredential credential;
        using (FileStream stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
        {
            string credPath = tokenPath;
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                [DriveService.Scope.Drive],
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
        }

        _driveService = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
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
    public File UpdateFile(string fileId, string newName = null, Stream newContent = null, string newMimeType = null)
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