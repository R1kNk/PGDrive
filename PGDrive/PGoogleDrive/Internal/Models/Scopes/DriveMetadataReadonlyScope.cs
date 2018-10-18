namespace PGoogleDrive.Internal.Models.Scopes
{
    class DriveMetadataReadonlyScope : PGScope
    {
        public DriveMetadataReadonlyScope() : base(Google.Apis.Drive.v3.DriveService.Scope.DriveMetadataReadonly)
        {
        }
    }
}
