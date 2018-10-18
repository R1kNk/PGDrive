using PGoogleDrive.Internal.Models.Search;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PGoogleDrive.Internal.Services
{
    public static class Search
    {
       static Dictionary<int, string> operators = new Dictionary<int, string>();
        static Dictionary<int, string> mimeTypes = new Dictionary<int, string>();


        static Search()
        {
            InitializeOperators();
            InitializeMimeTypes();
        }
        /// <summary>
        /// Searches files by name
        /// </summary>
        static public SearchBy ByName(Operators.Name Operator, string name)
        {
            var rt = operators.Where(p => p.Key.Equals((int)Operator)).SingleOrDefault();
            return new SearchByName(rt.Value, name);
        }
        /// <summary>
        /// Searches files by the fact of if there shared with you or not
        /// </summary>
        static public SearchBy IsShared(bool isShared)
        {
            return new SearchByIsShared("=", isShared);
        }
        /// <summary>
        /// Searches by starred files or not
        /// </summary>
        static public SearchBy IsStarred(bool isStarred)
        {
            return new SearchByIsStarred("=", isStarred);
        }
        /// <summary>
        /// Searches by Is file was trashed or not
        /// </summary>
        static public SearchBy IsTrashed(bool isTrashed)
        {
            return new SearchByIsStarred("=", isTrashed);
        }
        /// <summary>
        /// Searches by the last modified time
        /// </summary>
        static public SearchBy ByModifiedTime(Operators.ModifiedTime Operator, DateTime dateTime)
        {
            
            var rt = operators.Where(p => p.Key.Equals((int)Operator)).SingleOrDefault();
            string Time = dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);
            return new SearchByModifiedTime(rt.Value, Time);
        }
        /// <summary>
        /// Searches by last viewed by you timr
        /// </summary>
        static public SearchBy ByViewedByMeTime(Operators.ModifiedTime Operator, DateTime dateTime)
        {

            var rt = operators.Where(p => p.Key.Equals((int)Operator)).SingleOrDefault();
            string Time = dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);
            return new SearchByViewedByMeTime(rt.Value, Time);
        }
        /// <summary>
        /// Searches files from specific folder
        /// </summary>
        static public SearchBy InFolder(string folderIdOrAlias)
        {
            return new SearchInFolder(folderIdOrAlias);
        }
        /// <summary>
        /// Searches files which specific user owns
        /// </summary>
        static public SearchBy InOwners(string userOrGroupEmail)
        {
            return new SearchInOwners(userOrGroupEmail);
        }
        /// <summary>
        /// Searches files which specific user or group can read
        /// </summary>
        static public SearchBy InReaders(string userOrGroupEmail)
        {
            return new SearchInReaders(userOrGroupEmail);
        }
        /// <summary>
        /// Searches files which specific user or group can write
        /// </summary>
        static public SearchBy InWriters(string userEmail)
        {
            return new SearchInWriters(userEmail);
        }
        /// <summary>
        /// Searches files by Mime Type
        /// </summary>
        static public SearchBy ByMimeType(Operators.MimeType Operator, string value)
        {
            var rt = operators.Where(p => p.Key.Equals((int)Operator)).SingleOrDefault();
            return new SearchByMimeType(rt.Value, value);
        }
        /// <summary>
        /// Searches files by generic mime types
        /// </summary>
        /// <param name="mime">Contains a collection of a generic mime types</param>
        static public SearchBy ByMimeTypeGeneric(MimeTypes.Generic mime)
        {
            var mimeType = mimeTypes.Where(p => p.Key.Equals((int)mime)).SingleOrDefault();
            return new SearchByMimeType("contains", mimeType.Value);
        }
        /// <summary>
        /// Searches by google mime types
        /// </summary>
        static public SearchBy ByMimeTypeGoogle(Operators.MimeType Operator ,MimeTypes.Google mime)
        {
            var op = operators.Where(p => p.Key.Equals((int)Operator)).SingleOrDefault();
            var mimeType = mimeTypes.Where(p => p.Key.Equals((int)mime)).SingleOrDefault();
            return new SearchByMimeType(op.Value, mimeType.Value);
        }
        /// <summary>
        /// Searches by application mime types
        /// </summary>
        static public SearchBy ByMimeTypeApplication(Operators.MimeType Operator, MimeTypes.Application mime)
        {
            var op = operators.Where(p => p.Key.Equals((int)Operator)).SingleOrDefault();
            var mimeType = mimeTypes.Where(p => p.Key.Equals((int)mime)).SingleOrDefault();
            return new SearchByMimeType(op.Value, mimeType.Value);
        }
        /// <summary>
        /// Searches by audio mime types
        /// </summary>
        static public SearchBy ByMimeTypeAudio(Operators.MimeType Operator, MimeTypes.Audio mime)
        {
            var op = operators.Where(p => p.Key.Equals((int)Operator)).SingleOrDefault();
            var mimeType = mimeTypes.Where(p => p.Key.Equals((int)mime)).SingleOrDefault();
            return new SearchByMimeType(op.Value, mimeType.Value);
        }
        /// <summary>
        /// Searches by image mime types
        /// </summary>
        static public SearchBy ByMimeTypeImage(Operators.MimeType Operator, MimeTypes.Image mime)
        {
            var op = operators.Where(p => p.Key.Equals((int)Operator)).SingleOrDefault();
            var mimeType = mimeTypes.Where(p => p.Key.Equals((int)mime)).SingleOrDefault();
            return new SearchByMimeType(op.Value, mimeType.Value);
        }
        /// <summary>
        /// Searches by text mime types
        /// </summary>
        static public SearchBy ByMimeTypeText(Operators.MimeType Operator, MimeTypes.Text mime)
        {
            var op = operators.Where(p => p.Key.Equals((int)Operator)).SingleOrDefault();
            var mimeType = mimeTypes.Where(p => p.Key.Equals((int)mime)).SingleOrDefault();
            return new SearchByMimeType(op.Value, mimeType.Value);
        }
        /// <summary>
        /// Searches by video mime types
        /// </summary>
        static public SearchBy ByMimeTypeVideo(Operators.MimeType Operator, MimeTypes.Video mime)
        {
            var op = operators.Where(p => p.Key.Equals((int)Operator)).SingleOrDefault();
            var mimeType = mimeTypes.Where(p => p.Key.Equals((int)mime)).SingleOrDefault();
            return new SearchByMimeType(op.Value, mimeType.Value);
        }

        static void InitializeOperators()
        {
            operators.Add(1, "contains");
            operators.Add(2, "in");
            operators.Add(3, "and");
            operators.Add(4, "or");
            operators.Add(5, "not");
            operators.Add(6, "=");
            operators.Add(7, "!=");
            operators.Add(8, "<");
            operators.Add(9, "<=");
            operators.Add(10, ">");
            operators.Add(11, ">=");
        }

        static void InitializeMimeTypes()
        {
            mimeTypes.Add(1, "application/vnd.google-apps.document");
            mimeTypes.Add(2, "application/vnd.google-apps.drawing");
            mimeTypes.Add(3, "application/vnd.google-apps.file");
            mimeTypes.Add(4, "application/vnd.google-apps.folder");
            mimeTypes.Add(5, "application/vnd.google-apps.form");
            mimeTypes.Add(6, "application/vnd.google-apps.fusiontable");
            mimeTypes.Add(7, "application/vnd.google-apps.map");
            mimeTypes.Add(8, "application/vnd.google-apps.presentation");
            mimeTypes.Add(9, "application/vnd.google-apps.script");
            mimeTypes.Add(10, "application/vnd.google-apps.site");
            mimeTypes.Add(11, "application/vnd.google-apps.spreadsheet");
            mimeTypes.Add(12, "application/atom+xml");
            mimeTypes.Add(13, "application/json");
            mimeTypes.Add(14, "application/javascript");
            mimeTypes.Add(15, "application/pdf");
            mimeTypes.Add(16, "application/xhtml+xml");
            mimeTypes.Add(17, "application/xml-dtd");
            mimeTypes.Add(18, "application/xop+xml");
            mimeTypes.Add(19, "application/zip");
            mimeTypes.Add(20, "application/xml");
            mimeTypes.Add(21, "audio/mp4");
            mimeTypes.Add(22, "audio/aac");
            mimeTypes.Add(23, "audio/mpeg");
            mimeTypes.Add(24, "audio/webm");
            mimeTypes.Add(25, "image/gif");
            mimeTypes.Add(26, "image/jpeg");
            mimeTypes.Add(27, "image/png");
            mimeTypes.Add(28, "image/svg+xml");
            mimeTypes.Add(29, "text/css");
            mimeTypes.Add(30, "text/html");
            mimeTypes.Add(31, "text/javascript");
            mimeTypes.Add(32, "text/php");
            mimeTypes.Add(33, "text/xml");
            mimeTypes.Add(34, "video/mpeg");
            mimeTypes.Add(35, "video/mp4");
            mimeTypes.Add(36, "video/webm");
            mimeTypes.Add(37, "application/");
            mimeTypes.Add(38, "application/vnd.google-apps");
            mimeTypes.Add(39, "audio/");
            mimeTypes.Add(40, "image/");
            mimeTypes.Add(41, "text/");
        }
    }
}
