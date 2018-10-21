using PGoogleDrive.Internal.Models.Scopes;


namespace PGoogleDrive
{
    /// <summary>
    /// Static class for getting all concrete classes of abstract Scope
    /// that represents all existing scopes that Google Drive Api provides
    /// </summary>
    public static class PGScopes
    {
        /// <summary>
        /// View and manage the files in your Google Drive
        /// </summary>
        public static PGScope Drive { get => new DriveScope(); }
        /// <summary>
        /// View and manage Google Drive files and folders that you have opened or created with this app
        /// </summary>
        public static PGScope DriveFile { get => new DriveFileScope(); }
        /// <summary>
        /// View and manage its own configuration data in your Google Drive
        /// </summary>
        public static PGScope DriveAppdata { get => new DriveAppdataScope(); }
        /// <summary>
        /// View metadata for files in your Google Drive
        /// </summary>
        public static PGScope DriveMetadataReadonly { get => new DriveMetadataReadonlyScope(); }
        /// <summary>
        /// View and manage metadata of files in your Google Drive
        /// </summary>
        public static PGScope DriveMetadata { get => new DriveMetadataScope(); }
        /// <summary>
        /// View the photos, videos and albums in your Google Photos
        /// </summary>
        public static PGScope DrivePhotosReadonly { get => new DrivePhotosReadonlyScope(); }
        /// <summary>
        /// View the files in your Google Drive
        /// </summary>
        public static PGScope DriveReadonly { get => new DriveReadonlyScope(); }
        /// <summary>
        /// Modify your Google Apps Script scripts' behavior
        /// </summary>
        public static PGScope DriveScripts { get => new DriveScriptsScope(); }

    }
}
