namespace PGoogleDrive.Internal.Models.Search
{
    public class Operators
    {

        public enum Name
        {
            Contains = 1,
            Equal = 6,
            NotEqual = 7
        }

        public enum MimeType
        {
            Contains = 1,
            Equal = 6,
            NotEqual = 7
        }

        public enum IsStarred
        {
          Equal = 6,
          NotEqual = 7
        }

        public enum IsTrashed
        {
            Equal = 6,
            NotEqual = 7
        }

        public enum IsShared
        {
            Equal = 6,
            NotEqual = 7
        }

        public enum ModifiedTime
        {
            Less = 8,
            LessOrEqual = 9,
            Equal = 6,
            NotEqual = 7,
            More = 10,
            MoreOrEqual = 11
        }

        public enum ViewedByMeTime
        {
            Less = 8,
            LessOrEqual = 9,
            Equal = 6,
            NotEqual = 7,
            More = 10,
            MoreOrEqual = 11
        }
    }
}
