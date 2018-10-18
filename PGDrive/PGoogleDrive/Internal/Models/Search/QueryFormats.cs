using System;

namespace PGoogleDrive.Internal.Models.Search
{
    static class QueryFormats
    {
        static public string GetBoolQuery(string FieldName, string Operator, bool Value)
        {
            return String.Format("({0} {1} {2})", FieldName, Operator, Value.ToString());
        }

        static public string GetSharedWithMeQuery(bool Value)
        {
            if(Value)
            return String.Format("({0})", "sharedWithMe");
            else return "(not (not 'me' in owners))";

        }

        static public string GetStringQuery(string FieldName, string Operator, string Value)
        {
            return String.Format("({0} {1} '{2}')", FieldName, Operator, Value);
        }

        static public string GetInQuery(string FieldName, string Operator, string Value)
        {
            return String.Format("('{0}' {1} {2})", Value, Operator, FieldName);
        }
    }
}
