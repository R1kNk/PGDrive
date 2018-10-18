using Google.Apis.Requests;

namespace PGoogleDrive.Internal.Models.General
{
    /// <summary>
    /// Almost all methods of PGDrive class returns objects of this class with different bodies that Google Drive API returns 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PGDriveResult<T>
    {
        /// <summary>
        /// Is request to google drive was successful
        /// </summary>
        public bool isSuccess { get; private set; } 
        /// <summary>
        /// ResponseBody contains data that was returned to user via Google Drive api
        /// </summary>
        public T ResponseBody { get; private set; }
        /// <summary>
        /// If request wasn't successful - this object represents type and reason of error
        /// </summary>
        public  RequestError Error { get; private set; }

        /// <summary>
        /// </summary>
        public PGDriveResult()
        {
            isSuccess = true;
        }

        internal void SetIsSuccess(bool isSuccess)
        {
            this.isSuccess = isSuccess;
        }
        internal void SetResponseBody(T result)
        {
            ResponseBody = result;
        }

        void SetError(RequestError error)
        {
            Error = error;
        }

        internal void InitializeError(Google.GoogleApiException exception)
        {
            isSuccess = false;
            SetError(exception.Error);
        }

        internal PGDriveResult<U> CopyResult<U>()
        {
            PGDriveResult<U> pGDriveResult = new PGDriveResult<U>();
            pGDriveResult.isSuccess = this.isSuccess;
            pGDriveResult.SetError(this.Error);
            return pGDriveResult;
        } 
    }

}
