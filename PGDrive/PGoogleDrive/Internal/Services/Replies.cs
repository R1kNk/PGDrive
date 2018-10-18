using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using PGoogleDrive.Internal.Models;
using System.Collections.Generic;

namespace PGoogleDrive.Internal.Services
{
    public class Replies : ServiceModule
    {
        public Replies(DriveService service) : base(service)
        {
            DefaultReplyFields = "id, author, content, deleted, htmlContent";
            DefaultReplySizePerRequest = 100;
            DefaultGetFieldsOnResponse = "nextPageToken, replies(id, author, content, deleted, htmlContent)";

        }

        string DefaultReplyFields { get; set; }
        string DefaultGetFieldsOnResponse { get; set; }
        int DefaultReplySizePerRequest { get; set; }

        const int DefaultMaxRepliesCount = -1;
        /// <summary>
        /// Creates a comment to a file
        /// </summary>
        /// <param name="fileId">Id of a file</param>
        /// <param name="commentId">Id of a comment</param>
        /// <param name="body">Body of future reply</param>
        /// <returns>PGDriveResult with create reply in response body</returns>
        public PGDriveResult<Reply> CreateReply(string fileId, string commentId, Reply body)
        {
            PGDriveResult<Reply> pGDriveResult = new PGDriveResult<Reply>();
            try
            {
                var createRequest = driveService.Replies.Create(body, fileId, commentId);
                createRequest.Fields = DefaultReplyFields;
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
        /// Creates a comment to a file
        /// </summary>
        /// <param name="fileId">Id of a file</param>
        /// <param name="commentId">Id of a comment</param>
        /// <param name="content">Text of future reply</param>
        /// <returns>PGDriveResult with create reply in response body</returns>
        public PGDriveResult<Reply> CreateReply(string fileId, string commentId, string content)
        {
            PGDriveResult<Reply> pGDriveResult = new PGDriveResult<Reply>();
            try
            {
                var createRequest = driveService.Replies.Create(new Reply() {Content = content }, fileId, commentId);
                createRequest.Fields = DefaultReplyFields;
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
        /// Creates a comment to a file
        /// </summary>
        /// <param name="fileId">Id of a file</param>
        /// <param name="commentId">Id of a comment</param>
        /// <param name="replyId">Id of reply to delete</param>
        /// <returns>PGDriveResult with bool which shows did reply was deleted or not</returns>
        public PGDriveResult<bool> DeleteReply(string fileId, string commentId, string replyId)
        {
            PGDriveResult<bool> pGDriveResult = new PGDriveResult<bool>();
            try
            {
                var createRequest = driveService.Replies.Delete(fileId, commentId, replyId);
                createRequest.Fields = DefaultReplyFields;
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
        /// Get replies from specific comment
        /// </summary>
        /// <param name="fileId">Id of a file</param>
        /// <param name="commentId">Id of a comment</param>
        /// <param name="maxRepliesCount">Maximum count of replies you need</param>
        /// <returns>PGDriveResult with collection of getted replies in response body</returns>
        public PGDriveResult<IList<Reply>> GetReplies(string fileId, string commentId, int maxRepliesCount = DefaultMaxRepliesCount)
        {
            if (maxRepliesCount <= 0 && maxRepliesCount != DefaultMaxRepliesCount)
            {
                maxRepliesCount = DefaultMaxRepliesCount;
            }
            List<Reply> replies = new List<Reply>();
            PGDriveResult<IList<Reply>> pGDriveResult = new PGDriveResult<IList<Reply>>();
            var listRequest = driveService.Replies.List(fileId, commentId);
            try
            {
                listRequest.Fields = DefaultGetFieldsOnResponse;
                listRequest.PageSize = DefaultReplySizePerRequest;
                if (maxRepliesCount != DefaultMaxRepliesCount)
                {
                    if (maxRepliesCount < DefaultReplySizePerRequest)
                    {
                        listRequest.PageSize = maxRepliesCount;
                        maxRepliesCount = 0;
                    }
                    else
                    {
                        maxRepliesCount -= DefaultReplySizePerRequest;
                    }
                }
                var result = listRequest.Execute();
                if (result.Replies != null)
                {
                    replies.AddRange(result.Replies);
                    if (result.Replies.Count < DefaultReplySizePerRequest)
                    {
                        maxRepliesCount = 0;
                    }
                }
                while (maxRepliesCount != 0)
                {
                    if (!string.IsNullOrWhiteSpace(result.NextPageToken))
                    {
                        listRequest = driveService.Replies.List(fileId, commentId);
                        listRequest.PageToken = result.NextPageToken;
                        listRequest.PageSize = DefaultReplySizePerRequest;
                        listRequest.Fields = DefaultGetFieldsOnResponse;
                        if (maxRepliesCount != DefaultMaxRepliesCount)
                        {
                            if (maxRepliesCount < DefaultReplySizePerRequest)
                            {
                                listRequest.PageSize = maxRepliesCount;
                                maxRepliesCount = 0;
                            }
                            else
                            {
                                maxRepliesCount -= DefaultReplySizePerRequest;
                            }
                        }
                        result = listRequest.Execute();
                        if (result.Replies != null)
                        {
                            replies.AddRange(result.Replies);
                        }

                    }
                    else break;
                }
                pGDriveResult.SetResponseBody(replies);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Gets reply by id
        /// </summary>
        /// <param name="fileId">Id of a fiile</param>
        /// <param name="commentId">Id of a comment</param>
        /// <param name="replyId">Id of a reply</param>
        /// <returns>PGDriveResult with getted reply</returns>
        public PGDriveResult<Reply> GetReplyById(string fileId, string commentId, string replyId)
        {
            PGDriveResult<Reply> pGDriveResult = new PGDriveResult<Reply>();
            try
            {
                var getReplyRequest = driveService.Replies.Get(fileId, commentId, replyId);
                getReplyRequest.Fields = DefaultReplyFields;
                var reply = getReplyRequest.Execute();
                pGDriveResult.SetResponseBody(reply);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Updates a  reply
        /// </summary>
        /// <param name="fileId">Id of a fiile</param>
        /// <param name="commentId">Id of a comment</param>
        /// <param name="replyId">Id of a reply</param>
        /// <param name="body">Future body of a reply to update</param>
        /// <returns>PGDriveResult with updated reply</returns>
        public PGDriveResult<Reply> UpdateReply(string fileId, string commentId, string replyId, Reply body)
        {
            PGDriveResult<Reply> pGDriveResult = new PGDriveResult<Reply>();
            try
            {
                var updateReplyRequest = driveService.Replies.Update(body, fileId, commentId, replyId);
                updateReplyRequest.Fields = DefaultReplyFields;
                var reply = updateReplyRequest.Execute();
                pGDriveResult.SetResponseBody(reply);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
        /// <summary>
        /// Updates a  reply
        /// </summary>
        /// <param name="fileId">Id of a fiile</param>
        /// <param name="commentId">Id of a comment</param>
        /// <param name="replyId">Id of a reply</param>
        /// <param name="content">Future text of a reply to update</param>
        /// <returns>PGDriveResult with updated reply</returns>
        public PGDriveResult<Reply> UpdateReply(string fileId, string commentId, string replyId, string content)
        {
            PGDriveResult<Reply> pGDriveResult = new PGDriveResult<Reply>();
            try
            {
                var updateReplyRequest = driveService.Replies.Update(new Reply() { Content = content}, fileId, commentId, replyId);
                updateReplyRequest.Fields = DefaultReplyFields;
                var reply = updateReplyRequest.Execute();
                pGDriveResult.SetResponseBody(reply);
                return pGDriveResult;
            }
            catch (Google.GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }
    }
}
