namespace PGoogleDrive.Internal.Models.Search
{
    class SearchByIsStarred : SearchByValue<bool>
    {
        public SearchByIsStarred(string Operator, bool Value) : base("starred", Operator, Value)
        {
        }

        protected override string BuildQuery()
        {
            return QueryFormats.GetBoolQuery(FieldName, Operator, Value);
        }
    }
}
