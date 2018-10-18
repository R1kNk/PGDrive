namespace PGoogleDrive.Internal.Models.Search
{
    class SearchByViewedByMeTime : SearchByValue<string>
    {
        public SearchByViewedByMeTime(string Operator, string Value) : base("viewedByMeTime", Operator, Value)
        {
        }

        protected override string BuildQuery()
        {
            return  QueryFormats.GetStringQuery(FieldName, Operator, Value);
        }
    }
}
