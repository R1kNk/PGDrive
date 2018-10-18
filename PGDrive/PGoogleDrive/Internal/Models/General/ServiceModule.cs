using Google.Apis.Drive.v3;

namespace PGoogleDrive.Internal.Models.General
{
    /// <summary>
    /// Module that represents parent for all modules with functionality of PGDrive
    /// </summary>
    public abstract class ServiceModule
    {
        /// <summary>
        /// Drive service of Google rive api
        /// </summary>
        protected DriveService driveService { get; set; }

        /// <summary>
        /// Initializes module with drive service
        /// </summary>
        /// <param name="service"></param>
        public ServiceModule(DriveService service)
        {
            driveService = service;
        }
    }
}
