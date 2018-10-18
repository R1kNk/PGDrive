namespace PGoogleDrive.Internal.Models.Scopes
{
    class DrivePhotosReadonlyScope : PGScope
    {
        public DrivePhotosReadonlyScope() : base(Google.Apis.Drive.v3.DriveService.Scope.DrivePhotosReadonly)
        {
        }
    }
}
