namespace PGoogleDrive.Internal.Models.Search
{
    public class MimeTypes
    {
       public enum Google
        {
            Docs = 1,
            Drawing = 2,
            DriveFile = 3,
            DriveFolder = 4,
            Forms = 5,
            FusionTables = 6,
            MyMaps = 7,
            Slides = 8,
            AppScripts = 9,
            Sites = 10,
            Sheets = 11
        }

        public enum Application
        {
            Atom = 12,
            JSON = 13,
            JavaScript = 14,
            Pdf = 15,
            XHTML = 16,
            DTD = 17,
            XOP = 18,
            ZIP = 19,
            XML = 20
        }

        public enum Audio
        {
            MP4 = 21,
            AAC = 22,
            MP3 = 23,
            WebM = 24
        }

        public enum Image
        {
            Gif = 25,
            JPEG,
            PNG,
            SVG
        }

        public enum Text
        {
            CSS = 29,
            HTML,
            JavaScript,
            PHP,
            XML = 33
        }

        public enum Video
        {
            MPEG = 34,
            MP4,
            WebM
        }

        /// <summary>
        /// Generic group of mime types
        /// </summary>
        public enum Generic
        {
            Application = 37,
            Google,
            Audio,
            Image,
            Text
        }
    }
}
    