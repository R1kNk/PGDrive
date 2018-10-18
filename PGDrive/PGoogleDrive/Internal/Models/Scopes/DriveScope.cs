namespace PGoogleDrive.Internal.Models.Scopes
{
    class DriveScope : PGScope
    {
        public DriveScope() : base(Google.Apis.Drive.v3.DriveService.Scope.Drive)
        {
        }
    }
}
