namespace PGoogleDrive.Internal.Models.Scopes
{
    class DriveMetadataScope : PGScope
    {
        public DriveMetadataScope() : base(Google.Apis.Drive.v3.DriveService.Scope.DriveMetadata)
        {
        }
    }
}
