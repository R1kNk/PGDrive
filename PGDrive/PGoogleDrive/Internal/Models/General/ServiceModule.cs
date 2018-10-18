using Google.Apis.Drive.v3;

namespace PGoogleDrive.Internal.Models.General
{
    public abstract class ServiceModule
    {
        protected DriveService driveService { get; set; }

        public ServiceModule(DriveService service)
        {
            driveService = service;
        }
    }
}
