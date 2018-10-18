using Google.Apis.Drive.v3;

namespace PGoogleDrive.Internal.Models
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
