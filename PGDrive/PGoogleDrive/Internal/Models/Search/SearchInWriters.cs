namespace PGoogleDrive.Internal.Models.Search
{
    class SearchInWriters : SearchByValue<string>
    {
        public SearchInWriters(string Value) : base("writers", "in", Value)
        {
        }

        protected override string BuildQuery()
        {
            return QueryFormats.GetInQuery(FieldName, Operator, Value);
        }
    }
}
