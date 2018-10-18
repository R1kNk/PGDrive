namespace PGoogleDrive.Internal.Models.Search
{
    class SearchInReaders : SearchByValue<string>
    {
        public SearchInReaders(string Value) : base("readers", "in", Value)
        {
        }

        protected override string BuildQuery()
        {
            return  QueryFormats.GetInQuery(FieldName, Operator, Value);
        }
    }
}
