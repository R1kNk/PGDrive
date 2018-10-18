namespace PGoogleDrive.Internal.Models.Scopes
{
    class DriveFileScope : PGScope
    {
        public DriveFileScope() : base(Google.Apis.Drive.v3.DriveService.Scope.DriveFile)
        {
        }
    }
}
