namespace PGoogleDrive.Internal.Models.Scopes
{
    class DriveReadonlyScope : PGScope
    {
        public DriveReadonlyScope() : base(Google.Apis.Drive.v3.DriveService.Scope.DriveReadonly)
        {
        }
    }
}
