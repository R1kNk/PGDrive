namespace PGoogleDrive.Internal.Models.Search
{
    class SearchByIsShared : SearchByValue<bool>
    {
        protected override string BuildQuery()
        {
            return QueryFormats.GetSharedWithMeQuery(Value);
        }

        public SearchByIsShared(string Operator, bool Value) : base("sharedWithMe", Operator, Value)
        {

        }
    }
}
