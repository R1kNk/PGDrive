namespace PGoogleDrive.Internal.Models.Search
{
    class SearchByModifiedTime : SearchByValue<string>
    {
        public SearchByModifiedTime(string Operator, string Value) : base("modifiedTime", Operator, Value)
        {
        }

        protected override string BuildQuery()
        {
            return QueryFormats.GetStringQuery(FieldName, Operator, Value);
        }
    }
}
