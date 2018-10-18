using Google.Apis.Drive.v3;
using PGoogleDrive.Internal.Services;
using static Google.Apis.Drive.v3.DriveService;
using PGoogleDrive.Internal.Models.Scopes;

namespace PGoogleDrive
{
    public class PGDrive
    {
        public string ApplicationName { get; private set; }

        DriveService driveService { get; set; }

        /// <summary>
        /// Creates a new object of PGDrive object and authenticates user
        /// </summary>
        /// <param name="pGDriveConfigElementName">Name of element in PGDrive section in your config</param>
        /// <param name="scopes">Scopes provides permissions to use your drive. Use PGScopes static class,
        /// combine scopes using method And in scopes.
        /// </param>
        /// <param name="reCreateOAuthToken">Recreate your OAuth response token with new scopes you need?</param>
        /// <exception cref="NullReferenceException">Throws if configElement with such name wasn't founded</exception>
        public PGDrive(string pGDriveConfigElementName, PGScope scopes = null, bool reCreateOAuthToken = false)
        {
            Auth = new Auth();
            driveService = Auth.GetDriveService(pGDriveConfigElementName, scopes, reCreateOAuthToken);
            ApplicationName = Auth.ApplicationName;
            Permissions = new Permissions(driveService);
            Files = new Files(driveService);
            Comments = new Comments(driveService);
            Replies = new Replies(driveService);

        }

        /// <summary>
        /// Contains all methods and properties to work with permissions
        /// </summary>
        public Permissions Permissions { get; set; }
        /// <summary>
        /// Contains all methods to work with files and folders
        /// </summary>
        public Files Files { get; set; }
        /// <summary>
        /// Contains all methods to work with comments
        /// </summary>
        public Comments Comments { get; set; }
        /// <summary>
        /// Contains all methods to work with replies to comments
        /// </summary>
        public Replies Replies { get; set; }
        Auth Auth { get; set; }


    }
}
