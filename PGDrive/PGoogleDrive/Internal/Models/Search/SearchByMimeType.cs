namespace PGoogleDrive.Internal.Models.Search
{
    class SearchByMimeType : SearchByValue<string>
    {
        public SearchByMimeType(string Operator, string Value) : base("mimeType", Operator, Value)
        {
        }

        protected override string BuildQuery()
        {
            return QueryFormats.GetStringQuery(FieldName, Operator, Value);
        }
    }
}
