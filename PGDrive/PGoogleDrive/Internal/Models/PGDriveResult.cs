using Google.Apis.Requests;

namespace PGoogleDrive.Internal.Models
{
    public class PGDriveResult<T>
    {
        public bool isSuccess { get; private set; } 
        public T ResponseBody { get; private set; }
        public  RequestError Error { get; private set; }

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
