namespace PGoogleDrive.Internal.Models.General
{
    public class FileDownloadResult
    {
       public System.IO.MemoryStream FileMemoryStream { get; private set; }
       public string FileMimeType { get; private set; }
        public string Extension { get; private set; }

        public FileDownloadResult(System.IO.MemoryStream stream, string FileMimeType, string extension = null)
        {
            FileMemoryStream = stream;
            this.FileMimeType = FileMimeType;
            Extension = extension;
        }
    }
}
