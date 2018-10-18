using Google.Apis.Drive.v3;
using Google.Apis.Requests;
using System.Collections.Generic;

namespace PGoogleDrive.Internal.Models
{
    static class CustomExceptions
    {
        public static Google.GoogleApiException isNotAFolder(string id, DriveService service)
        {
            Google.GoogleApiException notAFolder = new Google.GoogleApiException(service.Name, "File is not a folder :" + id);
            RequestError error = new RequestError();
            error.Code = 400;
            error.Message = "File is not a folder: " + id;
            error.Errors = new List<SingleError>() { new SingleError() { Domain = "global", Location = "field", LocationType = "parameter", Message = "File is not a folder: " + id, Reason = "notAFolder" } };
            notAFolder.Error = error;
            return notAFolder;
        }

        public static Google.GoogleApiException isAFolder(string id, DriveService service)
        {
            Google.GoogleApiException notAFolder = new Google.GoogleApiException(service.Name, "File is a folder :" + id);
            RequestError error = new RequestError();
            error.Code = 400;
            error.Message = "File is a folder: " + id;
            error.Errors = new List<SingleError>() { new SingleError() { Domain = "global", Location = "field", LocationType = "parameter", Message = "File is a folder: " + id, Reason = "isAFolder" } };
            notAFolder.Error = error;
            return notAFolder;
        }
    }
}
