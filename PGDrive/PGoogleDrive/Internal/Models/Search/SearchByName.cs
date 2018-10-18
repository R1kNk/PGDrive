namespace PGoogleDrive.Internal.Models.Search
{
    class SearchByName : SearchByValue<string>
    {
        protected override string BuildQuery()
        {
            return QueryFormats.GetStringQuery(FieldName, Operator, Value);
        }

        public SearchByName(string Operator, string Value) : base("name", Operator, Value)
        {

        }
    }
}
