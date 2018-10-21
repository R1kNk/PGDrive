using Google.Apis.Drive.v3;
using PGoogleDrive.Internal.Services;
using PGoogleDrive.Internal.Models.Scopes;
using System;

namespace PGoogleDrive
{
    /// <summary>
    /// Main class of PGoogleDrive for interaction with Google drive.
    /// Contains all methods and modules to use.
    /// </summary>
    public class PGDrive
    {

        string ApplicationName { get; set; }

        DriveService driveService { get; set; }

        /// <summary>
        /// Static method that creates a new object of PGDrive class and authenticates user using OAuth2.0 authorization
        /// </summary>
        /// <param name="ConfigOAuthDriveName">Name of element in ApiKeyDrives collection of PGDrive section in your config</param>
        /// <param name="scopes"> USE PGScopes static class,
        /// combine scopes using method And in scope objects.
        /// Scopes provides permissions to use your drive.
        /// </param>
        /// <param name="reCreateOAuthToken">Recreates an access token for current drive if you want to change the scopes</param>
        /// <exception cref="NullReferenceException">Throws if elemtent of OAuthDrives config collection with such name wasn't founded</exception>
        public static PGDrive GetOAuthDrive(string ConfigOAuthDriveName, PGScope scopes = null, bool reCreateOAuthToken = false)
        {
            PGDrive newDrive = new PGDrive();
            newDrive.Auth = new Auth();
            newDrive.driveService = newDrive.Auth.GetOAuthDrive(ConfigOAuthDriveName, scopes, reCreateOAuthToken);
            newDrive.InitializeModules();
            return newDrive;

        }

        /// <summary>
        /// Static method that creates a new object of PGDrive class and authenticates user using ApiKey authorization
        /// </summary>
        /// <param name="ConfigApiKeyDriveName">Name of element in ApiKeyDrives collection of PGDrive section in your config</param>
        public static PGDrive GetApiKeyDrive (string ConfigApiKeyDriveName)
        {
            PGDrive newDrive = new PGDrive();
            newDrive.Auth = new Auth();
            newDrive.driveService = newDrive.Auth.GetApiKeyDrive(ConfigApiKeyDriveName);
            newDrive.InitializeModules();
            return newDrive;
        }

        internal PGDrive() {

        }
        void InitializeModules()
        {
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
