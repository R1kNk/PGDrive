using Google.Apis.Drive.v3;
using PGoogleDrive.Internal.Models;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using PGoogleDrive.Internal.Models.Search;
using System.Web;
using Microsoft.Win32;

namespace PGoogleDrive.Internal.Services
{
    public class Files : ServiceModule
    {
        public Files(DriveService service) : base(service)
        {
            DefaultGetFieldsOnResponse = "nextPageToken, files(id, name, mimeType, owners, parents, permissions, shared, trashed, webContentLink, webViewLink)";
            DefaultFileFieldsOnResponse = "id, name, mimeType, owners, parents, permissions, shared, trashed, webContentLink, webViewLink";
            DefaultPageSizePerRequest = 1000;
        }

        string DefaultGetFieldsOnResponse { get; set; }
        string DefaultFileFieldsOnResponse { get; set; }
        int DefaultPageSizePerRequest { get; set; }
        const int DefaultMaxFilesCount = -1;


        private PGDriveResult<IList<File>> GetFiles(string Q = null, int maxFilesCount = DefaultMaxFilesCount)
        {
            if (maxFilesCount <= 0 && maxFilesCount != DefaultMaxFilesCount)
            {
                maxFilesCount = DefaultMaxFilesCount;
            }
            List<File> files = new List<File>();
            PGDriveResult<IList<File>> pGDriveResult = new PGDriveResult<IList<File>>();
            FilesResource.ListRequest listRequest = driveService.Files.List();
            try
            {
                listRequest.Fields = DefaultGetFieldsOnResponse;
                listRequest.PageSize = DefaultPageSizePerRequest;
                if (Q != null)
                {
                    listRequest.Q = Q;
                }
                if (maxFilesCount != DefaultMaxFilesCount)
                {
                    if (maxFilesCount < DefaultPageSizePerRequest)
                    {
                        listRequest.PageSize = maxFilesCount;
                        maxFilesCount = 0;
                    }
                    else
                    {
                        maxFilesCount -= DefaultPageSizePerRequest;
                    }
                }
                var result = listRequest.Execute();
                if (result.Files != null)
                {
                    files.AddRange(result.Files);
                    if (result.Files.Count < DefaultPageSizePerRequest)
                    {
                        maxFilesCount = 0;
                    }
                }
                while (maxFilesCount != 0)
                {
                    if (!string.IsNullOrWhiteSpace(result.NextPageToken))
                    {
                        listRequest = driveService.Files.List();
                        listRequest.PageToken = result.NextPageToken;
                        listRequest.PageSize = DefaultPageSizePerRequest;
                        listRequest.Fields = DefaultGetFieldsOnResponse;
                        if (Q != null)
                        {
                            listRequest.Q = Q;
                        }
                        if (maxFilesCount != DefaultMaxFilesCount)
                        {
                            if (maxFilesCount < DefaultPageSizePerRequest)
                            {
                                listRequest.PageSize = maxFilesCount;
                                maxFilesCount = 0;
                            }
                            else
                            {
                                maxFilesCount -= DefaultPageSizePerRequest;
                            }
                        }
                        result = listRequest.Execute();
                        if (result.Files != null)
                        {
                            files.AddRange(result.Files);
                        }

                    }
                    else break;
                }
                pGDriveResult.SetResponseBody(files);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }

        /// <summary>
        /// Creates a folder at the drive
        /// </summary>
        /// <param name="name">Name of folder</param>
        /// <param name="parentFolderId">Id of parent folder for new folder</param>
        /// <returns>PGDriveResult which contains created folder metadata in response body</returns>
        public PGDriveResult<File> CreateFolder(string name, string parentFolderId = null)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                FilesResource.CreateRequest request;
                var fileMetadata = new File();
                fileMetadata.Name = name;
                fileMetadata.MimeType = "application/vnd.google-apps.folder";
                if (parentFolderId != null)
                {
                    var folder = driveService.Files.Get(parentFolderId).Execute();
                    if (!folder.MimeType.Contains("folder"))
                    {

                        throw CustomExceptions.isNotAFolder(parentFolderId, driveService);
                    }
                    fileMetadata.Parents = new List<string>() { parentFolderId };
                }
                request = driveService.Files.Create(fileMetadata);
                request.Fields = DefaultFileFieldsOnResponse;
                var file = request.Execute();

                pGDriveResult.SetResponseBody(file);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Uploads a file to a drive
        /// </summary>
        /// <param name="stream">Stream of your file to upload</param>
        /// <param name="contentType">MimeType of a file</param>
        /// <param name="name">Future name of the file at drive</param>
        /// <param name="parentFolderId">Id of parent folder for a new file</param>
        /// <returns>PGDriveResult which contains uploaded file metadata in response body</returns>
        public PGDriveResult<File> CreateFile(System.IO.Stream stream, string contentType, string name, string parentFolderId = null)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                FilesResource.CreateMediaUpload request;
                File body = new File();
                if (parentFolderId != null)
                {
                    var folder = driveService.Files.Get(parentFolderId).Execute();
                    if (!folder.MimeType.Contains("folder"))
                    {

                        throw CustomExceptions.isNotAFolder(parentFolderId, driveService);
                    }
                    body.Parents = new List<string>() { parentFolderId };
                }
                request = driveService.Files.Create(body, stream, contentType);
                request.Fields = DefaultFileFieldsOnResponse;
                request.Upload();
                var file = request.ResponseBody;
                pGDriveResult.SetResponseBody(file);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Uploads a file to a drive
        /// </summary>
        /// <param name="stream">Stream of your file to upload</param>
        /// <param name="contentType">MimeType of a file</param>
        /// <param name="body">Metadata of future file</param>
        /// <param name="parentFolderId">Id of parent folder for a new file</param>
        /// <returns>PGDriveResult which contains uploaded file metadata in response body</returns>
        public PGDriveResult<File> CreateFile(System.IO.Stream stream, string contentType, File body, string parentFolderId = null)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                FilesResource.CreateMediaUpload request;


                    if (parentFolderId != null)
                    {
                        var folder = driveService.Files.Get(parentFolderId).Execute();
                        if (!folder.MimeType.Contains("folder"))
                        {

                            throw CustomExceptions.isNotAFolder(parentFolderId, driveService);
                        }
                        body.Parents = new List<string>() { parentFolderId };
                    }
                    request = driveService.Files.Create(body, stream, contentType);
                    request.Fields = DefaultFileFieldsOnResponse;
                    request.Upload();
                var file = request.ResponseBody;
                pGDriveResult.SetResponseBody(file);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Uploads a fiile to a drive
        /// </summary>
        /// <param name="filePath">Path to file which you want to upload</param>
        /// <param name="name">Future name of the file</param>
        /// <param name="parentFolderId"></param>
        /// <returns>PGDriveResult which contains created folder metadata in response body</returns>
        public PGDriveResult<File> CreateFile(string filePath, string name = null, string parentFolderId = null)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                FilesResource.CreateMediaUpload request;
                filePath = System.IO.Path.GetFullPath(filePath);
                using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    var fileMetadata = new File();
                    if (name != null)
                    {
                        fileMetadata.Name = name;
                    }
                    if (parentFolderId != null)
                    {
                        var folder = driveService.Files.Get(parentFolderId).Execute();
                        if (!folder.MimeType.Contains("folder"))
                        {

                            throw CustomExceptions.isNotAFolder(parentFolderId, driveService);
                        }
                        fileMetadata.Parents = new List<string>() { parentFolderId };
                    }
                    string mimeType = MimeMapping.GetMimeMapping(filePath);
                    request = driveService.Files.Create(fileMetadata, stream, mimeType);
                    request.Fields = DefaultFileFieldsOnResponse;
                    request.Upload();
                }
                var file = request.ResponseBody;
                pGDriveResult.SetResponseBody(file);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Gets all files from a drive using optional search query
        /// </summary>
        /// <param name="searchPattern">Use static class Search for getting all needed SearchBy classes.
        /// Combine them using And, Or, Not methods in them to create a search query
        /// </param>
        /// <param name="maxFilesCount">Maximum files you need to get</param>
        /// <returns></returns>
        public PGDriveResult<IList<File>> GetFiles(SearchBy searchPattern = null, int maxFilesCount = DefaultMaxFilesCount)
        {
            string QPattern = null;
            if(searchPattern != null)
            {
                QPattern = searchPattern.Query;
            }
            var result = GetFiles(QPattern, maxFilesCount);
            return result;
        }
        /// <summary>
        /// Gets file by it id
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder</param>
        /// <returns></returns>
        public PGDriveResult<File> GetFileById(string fileOrFolderId)
        {
            List<File> files = new List<File>();
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            FilesResource.ListRequest listRequest = driveService.Files.List();
            try
            {
               var request =  driveService.Files.Get(fileOrFolderId);
                request.Fields = DefaultFileFieldsOnResponse;
                var result = request.Execute();
               pGDriveResult.SetResponseBody(result);
               return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Gets all files from folder using optional search query
        /// </summary>
        /// <param name="folderId">Id of a folder</param>
        /// <param name="searchPattern">Use static class Search for getting all needed SearchBy classes.
        /// Combine them using And, Or, Not methods in them to create a search query
        /// </param>
        /// <param name="maxFilesCount">Maximum files you need to get</param>
        /// <returns>PGDriveResult which contains collection of getted files  in response body</returns>
        public PGDriveResult<IList<File>> GetFilesFromFolder(string folderId, SearchBy searchPattern = null, int maxFilesCount = DefaultMaxFilesCount)
        {
            string QPattern = null;
            if (searchPattern != null)
            {
                QPattern = Search.InFolder(folderId).And(searchPattern).Query;

            }
            else { QPattern = Search.InFolder(folderId).Query; }
            var result = GetFiles(QPattern, maxFilesCount);
            return result;
        }
        /// <summary>
        /// Gets all files with specific name using optional search query
        /// </summary>
        /// <param name="fileOrFolderName">Name of a file or folder to search</param>
        /// <param name="searchPattern">Use static class Search for getting all needed SearchBy classes.
        /// Combine them using And, Or, Not methods in them to create a search query
        /// </param>
        /// <param name="maxFilesCount">Maximum files you need to get</param>
        /// <returns>PGDriveResult which contains collection of getted files  in response body</returns>
        public PGDriveResult<IList<File>> GetFilesByName(string fileOrFolderName, SearchBy searchPattern = null, int maxFilesCount = DefaultMaxFilesCount)
        {
            string QPattern = null;
            if (searchPattern != null)
            {
                QPattern = Search.ByName(Operators.Name.Equal, fileOrFolderName).And(searchPattern).Query;

            }
            else { QPattern = Search.ByName(Operators.Name.Equal, fileOrFolderName).Query; }
            var result = GetFiles(QPattern, maxFilesCount);
            return result;
        }
        /// <summary>
        /// Deletes file or folder with specific id
        /// </summary>
        /// <param name="fileOrFolderId">Id of a file or folder</param>
        /// <returns>PGDriveResult with bool in response body which shows did file was deleted or not</returns>
        public PGDriveResult<bool> DeleteFile (string fileOrFolderId)
        {
            PGDriveResult<bool> pGDriveResult = new PGDriveResult<bool>();
            try
            {
                var result = driveService.Files.Delete(fileOrFolderId).Execute();
                pGDriveResult.SetResponseBody(true);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Moves specific file to trash
        /// </summary>
        /// <param name="fileOrFolderId"></param>
        /// <returns>PGDriveResult with trashed file in response body</returns>
        public PGDriveResult<File> TrashFile(string fileOrFolderId)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                var result = UpdateFile(fileOrFolderId, isTrashed: true);
                return result;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Untrashes specific file
        /// </summary>
        /// <param name="fileOrFolderId"></param>
        /// <returns>PGDriveResult with untrashed file in response body</returns>
        public PGDriveResult<File> UnTrashFile(string fileOrFolderId)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                var result = UpdateFile(fileOrFolderId, isTrashed: false);
                return result;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Cleanes the trash from all files
        /// </summary>
        /// <param name="fileOrFolderId"></param>
        /// <returns>PGDriveResult with bool in response body which shows did trash was cleaned or not</returns>
        public PGDriveResult<bool> EmptyTrash(string fileOrFolderId)
        {
            PGDriveResult<bool> pGDriveResult = new PGDriveResult<bool>();
            try
            {
                driveService.Files.EmptyTrash().Execute();
                pGDriveResult.SetResponseBody(true);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Moves file from one folder to another
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder which yu want to move</param>
        /// <param name="newFolderId">Id of folder to which you want to move a file</param>
        /// <returns>PGDriveResult with moved file in response body</returns>
        public PGDriveResult<File> MoveFileToAntherFolder(string fileOrFolderId, string newFolderId)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                var getRequest = driveService.Files.Get(fileOrFolderId);
                var folderGetRequest = driveService.Files.Get(newFolderId);
                folderGetRequest.Fields = "mimeType";
                var folder = folderGetRequest.Execute();
                if (!folder.MimeType.Contains("folder"))
                {
                    throw CustomExceptions.isNotAFolder(newFolderId, driveService);
                }
                getRequest.Fields = "parents";
                var file = getRequest.Execute();
                var previousParents = String.Join(",", file.Parents);
                var updateRequest = driveService.Files.Update(new File(), fileOrFolderId);
                updateRequest.Fields = "id, parents";
                updateRequest.AddParents = newFolderId;
                updateRequest.RemoveParents = previousParents;
                file = updateRequest.Execute();
                pGDriveResult.SetResponseBody(file);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Removes all parents form a file or folder
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder</param>
        /// <returns>PGDriveResult contains file with removed parents in response body</returns>
        public PGDriveResult<File> RemoveAllParents(string fileOrFolderId)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                var getRequest = driveService.Files.Get(fileOrFolderId);
                getRequest.Fields = DefaultFileFieldsOnResponse;
                var file = getRequest.Execute();
                if (file.Parents == null)
                {
                    pGDriveResult.SetResponseBody(file);
                    return pGDriveResult;
                }
                string previousParents = String.Join(",", file.Parents);
                var updateRequest = driveService.Files.Update(new File(), fileOrFolderId);
                updateRequest.Fields = DefaultFileFieldsOnResponse;
                updateRequest.RemoveParents = previousParents;
                file = updateRequest.Execute();
                pGDriveResult.SetResponseBody(file);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Removes parents that collection of parents contain from a file
        /// </summary>
        /// <param name="fileOrFolderId">Id of a file or folder</param>
        /// <param name="parents">Collection of folders Id's which would be removed from a file</param>
        /// <returns>PGDriveResult contains file with removed parents in response body</returns>
        public PGDriveResult<File> RemoveParents(string fileOrFolderId, IList<string> parents)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                var getRequest = driveService.Files.Get(fileOrFolderId);
                getRequest.Fields = DefaultFileFieldsOnResponse;
                var file = getRequest.Execute();
                string parentsToDelete = String.Join(",", parents);
                var updateRequest = driveService.Files.Update(new File(), fileOrFolderId);
                updateRequest.Fields = DefaultFileFieldsOnResponse;
                updateRequest.RemoveParents = parentsToDelete;
                file = updateRequest.Execute();
                pGDriveResult.SetResponseBody(file);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Adds parents to a file
        /// </summary>
        /// <param name="fileOrFolderId">Id of a file or folder to adds parents to</param>
        /// <param name="parents">Collection of folders Id's which would be added as parents to a file</param>
        /// <returns>PGDriveResult contains file with added parents in response body</returns>
        public PGDriveResult<File> AddParents(string fileOrFolderId, IList<string> parents)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                var getRequest = driveService.Files.Get(fileOrFolderId);
                getRequest.Fields = DefaultFileFieldsOnResponse;
                var file = getRequest.Execute();
                string parentsToDelete = String.Join(",", parents);
                var updateRequest = driveService.Files.Update(new File(), fileOrFolderId);
                updateRequest.Fields = DefaultFileFieldsOnResponse;
                updateRequest.AddParents = parentsToDelete;
                file = updateRequest.Execute();
                pGDriveResult.SetResponseBody(file);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Downloads a file from a drive
        /// </summary>
        /// <param name="fileId">Id of file</param>
        /// <returns>PGDriveResult with a FileDownloadResult which contains memory stream of downloaded file, mimeType
        /// and it's extension
        /// </returns>
        public PGDriveResult<FileDownloadResult> DownloadFile(string fileId)
        {
            PGDriveResult<FileDownloadResult> pGDriveResult = new PGDriveResult<FileDownloadResult>();
            try
            {
                var fileRequest = driveService.Files.Get(fileId);
                fileRequest.Fields = DefaultFileFieldsOnResponse;
                var fileResponse = fileRequest.Execute();
                if (fileResponse.MimeType.Contains("folder"))
                {
                    throw CustomExceptions.isAFolder(fileId, driveService);
                }
                var request = driveService.Files.Get(fileId);
                var stream = new System.IO.MemoryStream();
                request.Download(stream);
                if (stream.Length != 0)
                {
                    string mimeType = fileResponse.MimeType;
                    string ext = GetDefaultExtension(mimeType);
                    FileDownloadResult fileDownloadResult = new FileDownloadResult(stream, mimeType,ext);
                    pGDriveResult.SetResponseBody(fileDownloadResult);
                }
                else
                {
                    pGDriveResult.SetResponseBody(null);
                    pGDriveResult.SetIsSuccess(false);
                }
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Copy file and optional change it's metadata
        /// </summary>
        /// <param name="fileId">Id of file to copy</param>
        /// <param name="bodyChanges">Changed file metadata</param>
        /// <returns>PGDriveResult with a bool which shows did file was copied in response body</returns>
        public PGDriveResult<bool> CopyFile(string fileId, File bodyChanges=null)
        {
            PGDriveResult<bool> pGDriveResult = new PGDriveResult<bool>();
            try
            {
                if(bodyChanges == null) { bodyChanges = new File(); }
                var request = driveService.Files.Copy(bodyChanges, fileId);
                request.Execute();
                pGDriveResult.SetResponseBody(true);
                return pGDriveResult;   
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Copy a file
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="name"></param>
        /// <param name="mimeType"></param>
        /// <param name="isStarred"></param>
        /// <param name="isTrashed"></param>
        /// <returns>PGDriveResult with a bool which shows did file was copied in response body</returns>
        public PGDriveResult<bool> CopyFile(string fileId, string name = null, string mimeType = null, bool? isStarred = null, bool? isTrashed = null)
        {
            File body = new File();
            if (name != null) body.Name = name;
            if (mimeType != null) body.MimeType = mimeType;
            var result = CopyFile(fileId, body);
            if (isStarred != null) body.Starred = isStarred;
            if (isTrashed != null) body.Trashed = isTrashed;
            return result;
        }
        /// <summary>
        /// Updates a file
        /// </summary>
        /// <param name="fileId">Id of file</param>
        /// <param name="bodyUpdates">Changes of file metadata</param>
        /// <returns>PGDriveResult with updated file in response body</returns>
        public PGDriveResult<File> UpdateFile(string fileId, File bodyUpdates)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                var getFileRequest = driveService.Files.Get(fileId);
                getFileRequest.Fields = DefaultFileFieldsOnResponse;
                var file = getFileRequest.Execute();
                if (bodyUpdates == null) { bodyUpdates = file; }

                var request = driveService.Files.Update(bodyUpdates, fileId);

                var result = request.Execute();
                pGDriveResult.SetResponseBody(result);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Updates a file
        /// </summary>
        /// <param name="fileId">Id of file</param>
        /// <param name="bodyUpdates">Changes of file metadata</param>
        /// <param name="filePath">Path to a file which would be a content to update</param>
        /// <returns>PGDriveResult with updated file in response body</returns>
        public PGDriveResult<File> UpdateFile(string fileId, File body, string filePath)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                filePath = System.IO.Path.GetFullPath(filePath);
                var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                string mimeType = MimeMapping.GetMimeMapping(filePath);
                if(body==null)
                {
                    body = new File();
                }
                var result = UpdateFile(fileId, stream, mimeType, body);

                return result;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Updates a file
        /// </summary>
        /// <param name="fileId">Id of file</param>
        /// <param name="stream">Stream of a file which would be a content to update</param>
        /// <param name="contentType">Mime type of a file stream</param>
        /// <param name="body">Optional body with metadata file</param>
        /// <returns>PGDriveResult with updated file in response body</returns>
        public PGDriveResult<File> UpdateFile(string fileId, System.IO.Stream stream, string contentType, File body=null)
        {
            PGDriveResult<File> pGDriveResult = new PGDriveResult<File>();
            try
            {
                var getFileRequest = driveService.Files.Get(fileId);
                getFileRequest.Fields = DefaultFileFieldsOnResponse;
                var file = getFileRequest.Execute();
                if (body == null) { body = file; }
                using (stream)
                {
                    var request = driveService.Files.Update(body, fileId, stream, contentType);
                    var res = request.Upload();
                }
                getFileRequest = driveService.Files.Get(fileId);
                getFileRequest.Fields = DefaultFileFieldsOnResponse;
                file = getFileRequest.Execute();
                pGDriveResult.SetResponseBody(file);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Updates a file
        /// </summary>
        /// <param name="fileId">Id of a file</param>
        /// <param name="filePath">Path to a file which would be a content to update</param>
        /// <param name="name">Future name of a file</param>
        /// <param name="mimeType">Mime type of a file</param>
        /// <param name="isStarred">Would be file trashed</param>
        /// <param name="isTrashed">Would be file starred</param>
        /// <returns>PGDriveResult with updated file in response body</returns>
        public PGDriveResult<File> UpdateFile(string fileId, string filePath= null, string name = null, string mimeType = null, bool? isStarred = null, bool? isTrashed = null)
        {
            File body = new File();
            if (name != null) body.Name = name;
            if (mimeType != null) body.MimeType = mimeType;
            if (isStarred != null) body.Starred = isStarred;
            if (isTrashed != null) body.Trashed = isTrashed;
            PGDriveResult<File> result = new PGDriveResult<File>();
            if (filePath != null)
            {
                filePath = System.IO.Path.GetFullPath(filePath);
                result = UpdateFile(fileId, body, filePath);
            }
            else
            {
                result = UpdateFile(fileId, body);
            }
            return result;
        }


        public string GetDefaultExtension(string mimeType)
        {
            string defaultExt;
            RegistryKey key;
            object value;
            key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            value = key != null ? key.GetValue("Extension", null) : null;
            defaultExt = value != null ? value.ToString() : string.Empty;
            return defaultExt;
        }

    }
}
