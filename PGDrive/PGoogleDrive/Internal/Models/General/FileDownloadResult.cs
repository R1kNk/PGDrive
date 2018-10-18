namespace PGoogleDrive.Internal.Models.General
{
    /// <summary>
    /// Model that Download file method returns
    /// </summary>
    public class FileDownloadResult
    {
       /// <summary>
       /// Memory stream of downloaded files.
       /// Could be converted into a file stream.
       /// </summary>
       public System.IO.MemoryStream FileMemoryStream { get; private set; }
       /// <summary>
       /// Mime type of downloaded file
       /// </summary>
       public string FileMimeType { get; private set; }
        /// <summary>
        /// EXtension of downloaded file
        /// </summary>
        public string Extension { get; private set; }

        /// <summary>
        /// Initializes result
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="FileMimeType"></param>
        /// <param name="extension"></param>
        public FileDownloadResult(System.IO.MemoryStream stream, string FileMimeType, string extension = null)
        {
            FileMemoryStream = stream;
            this.FileMimeType = FileMimeType;
            Extension = extension;
        }
    }
}
