namespace PGoogleDrive.Internal.Models.Search
{
    class SearchByIsTrashed : SearchByValue<bool>
    {
        public SearchByIsTrashed(string Operator, bool Value) : base("trashed", Operator, Value)
        {
        }

        protected override string BuildQuery()
        {
            return QueryFormats.GetBoolQuery(FieldName, Operator, Value);
        }
    }
}
