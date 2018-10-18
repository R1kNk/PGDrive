namespace PGoogleDrive.Internal.Models.Scopes
{
    class DriveAppdataScope : PGScope
    {
        public DriveAppdataScope() : base(Google.Apis.Drive.v3.DriveService.Scope.DriveAppdata)
        {
        }
    }
}
