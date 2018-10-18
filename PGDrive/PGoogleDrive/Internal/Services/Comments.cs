using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using PGoogleDrive.Internal.Models.General;
using System.Collections.Generic;

namespace PGoogleDrive.Internal.Services
{
    public class Comments : ServiceModule
    {
        public Comments(DriveService service) : base(service)
        {
            DefaultCommentFields = "id, author, content, deleted, htmlContent, replies, resolved";
            DefaultCommentSizePerRequest = 100;
            DefaultGetFieldsOnResponse = "nextPageToken, comments(id, author, content, deleted, htmlContent, replies, resolved)";
        }

        string DefaultCommentFields { get; set; }
        string DefaultGetFieldsOnResponse { get; set; }
        int DefaultCommentSizePerRequest { get; set; }

        const int DefaultMaxFilesCount = -1;
        /// <summary>
        /// Creates a comment to a file
        /// </summary>
        /// <param name="fileId">Id of a file</param>
        /// <param name="body">Metadata of a comment</param>
        /// <returns>PGDriveResult with a created comment in response body</returns>
        public PGDriveResult<Comment> CreateComment(string fileId, Comment body)
        {
            PGDriveResult<Comment> pGDriveResult = new PGDriveResult<Comment>();
            try
            {
                var createRequest = driveService.Comments.Create(body, fileId);
                createRequest.Fields = DefaultCommentFields;
                var result = createRequest.Execute();
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
        /// Creates a comment
        /// </summary>
        /// <param name="fileId">Id of a file</param>
        /// <param name="content">Text of a content</param>
        /// <returns>PGDriveResult with a created comment in response body</returns>
        public PGDriveResult<Comment> CreateComment(string fileId, string content)
        {
            PGDriveResult<Comment> pGDriveResult = new PGDriveResult<Comment>();
            try
            {
                var createRequest = driveService.Comments.Create(new Comment() {Content = content }, fileId);
                createRequest.Fields = DefaultCommentFields;
                var result = createRequest.Execute();
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
        /// Deletes a comment from file
        /// </summary>
        /// <param name="fileId">Id of a file</param>
        /// <param name="commentId">Id of a comment</param>
        /// <returns>PGDriveResult with a bool which shows did comment was deleted or not</returns>
        public PGDriveResult<bool> DeleteComment(string fileId, string commentId)
        {
            PGDriveResult<bool> pGDriveResult = new PGDriveResult<bool>();
            try
            {
                var createRequest = driveService.Comments.Delete(fileId, commentId);
                createRequest.Fields = DefaultCommentFields;
                var result = createRequest.Execute();
                if (string.IsNullOrEmpty(result))
                    pGDriveResult.SetResponseBody(true);
                else pGDriveResult.SetResponseBody(false);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Gets all comments form file
        /// </summary>
        /// <param name="fileId">Id of file</param>
        /// <param name="maxCommentsCount">Maximum count of comments which you want to get</param>
        /// <returns>PGDriveResult with collection of getted comments</returns>
        public PGDriveResult<IList<Comment>> GetComments(string fileId, int maxCommentsCount = DefaultMaxFilesCount)
        {
            if (maxCommentsCount <= 0 && maxCommentsCount != DefaultMaxFilesCount)
            {
                maxCommentsCount = DefaultMaxFilesCount;
            }
            List<Comment> comments = new List<Comment>();
            PGDriveResult<IList<Comment>> pGDriveResult = new PGDriveResult<IList<Comment>>();
            var listRequest = driveService.Comments.List(fileId);
            try
            {
                listRequest.Fields = DefaultGetFieldsOnResponse;
                listRequest.PageSize = DefaultCommentSizePerRequest;
                if (maxCommentsCount != DefaultMaxFilesCount)
                {
                    if (maxCommentsCount < DefaultCommentSizePerRequest)
                    {
                        listRequest.PageSize = maxCommentsCount;
                        maxCommentsCount = 0;
                    }
                    else
                    {
                        maxCommentsCount -= DefaultCommentSizePerRequest;
                    }
                }
                var result = listRequest.Execute();
                if (result.Comments != null)
                {
                    comments.AddRange(result.Comments);
                    if (result.Comments.Count < DefaultCommentSizePerRequest)
                    {
                        maxCommentsCount = 0;
                    }
                }
                while (maxCommentsCount != 0)
                {
                    if (!string.IsNullOrWhiteSpace(result.NextPageToken))
                    {
                        listRequest = driveService.Comments.List(fileId);
                        listRequest.PageToken = result.NextPageToken;
                        listRequest.PageSize = DefaultCommentSizePerRequest;
                        listRequest.Fields = DefaultGetFieldsOnResponse;
                        if (maxCommentsCount != DefaultMaxFilesCount)
                        {
                            if (maxCommentsCount < DefaultCommentSizePerRequest)
                            {
                                listRequest.PageSize = maxCommentsCount;
                                maxCommentsCount = 0;
                            }
                            else
                            {
                                maxCommentsCount -= DefaultCommentSizePerRequest;
                            }
                        }
                        result = listRequest.Execute();
                        if (result.Comments != null)
                        {
                            comments.AddRange(result.Comments);
                        }

                    }
                    else break;
                }
                pGDriveResult.SetResponseBody(comments);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Gets comment by id
        /// </summary>
        /// <param name="fileId">Id of a file</param>
        /// <param name="commentId">Id of a comment</param>
        /// <returns>PGDriveResult with a comment in response body</returns>
        public PGDriveResult<Comment> GetCommentById(string fileId, string commentId)
        {
            PGDriveResult<Comment> pGDriveResult = new PGDriveResult<Comment>();
            try
            {
                var getCommentRequest = driveService.Comments.Get(fileId, commentId);
                getCommentRequest.Fields = DefaultCommentFields;
                var comment = getCommentRequest.Execute();
                pGDriveResult.SetResponseBody(comment);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Updates comment
        /// </summary>
        /// <param name="fileId">Id of file</param>
        /// <param name="commentId">Id of a comment</param>
        /// <param name="body">Metadata of comment</param>
        /// <returns>PGDriveResult with updated comment in response body</returns>
        public PGDriveResult<Comment> UpdateComment(string fileId, string commentId, Comment body)
        {
            PGDriveResult<Comment> pGDriveResult = new PGDriveResult<Comment>();
            try
            {
                var updateCommentRequest = driveService.Comments.Update(body, fileId, commentId);
                updateCommentRequest.Fields = DefaultCommentFields;
                var comment = updateCommentRequest.Execute();
                pGDriveResult.SetResponseBody(comment);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Updates comment
        /// </summary>
        /// <param name="fileId">Id of file</param>
        /// <param name="commentId">Id of a comment</param>
        /// <param name="content">Text of a comment</param>
        /// <returns>PGDriveResult with updated comment in response body</returns>
        public PGDriveResult<Comment> UpdateComment(string fileId, string commentId, string content)
        {
            PGDriveResult<Comment> pGDriveResult = new PGDriveResult<Comment>();
            try
            {
                var result = UpdateComment(fileId, commentId, new Comment() { Content = content });
                return result;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }


    }
}
