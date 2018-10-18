namespace PGoogleDrive.Internal.Models.Scopes
{
    class DriveScriptsScope : PGScope
    {
        public DriveScriptsScope() : base(Google.Apis.Drive.v3.DriveService.Scope.DriveScripts)
        {
        }
    }
}
