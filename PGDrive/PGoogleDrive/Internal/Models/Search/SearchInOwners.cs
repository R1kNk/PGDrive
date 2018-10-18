namespace PGoogleDrive.Internal.Models.Search
{
    class SearchInOwners : SearchByValue<string>
    {
        public SearchInOwners(string Value) : base("owners", "in", Value)
        {
        }

        protected override string BuildQuery()
        {
            return QueryFormats.GetInQuery(FieldName, Operator, Value);
        }
    }
}
