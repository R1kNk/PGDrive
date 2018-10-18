namespace PGoogleDrive.Internal.Models.Search
{
    class SearchInFolder : SearchByValue<string>
    {
        public SearchInFolder(string Value) : base("parents", "in", Value)
        {
        }

        protected override string BuildQuery()
        {
            return  QueryFormats.GetInQuery(FieldName, Operator, Value);
        }
    }
}
