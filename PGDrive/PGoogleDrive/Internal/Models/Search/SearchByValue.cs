namespace PGoogleDrive.Internal.Models.Search
{
    abstract class SearchByValue<T> : SearchBy
    {
        protected T Value { get; set; }
        public SearchByValue(string FieldName,string Operator, T Value) : base(FieldName, Operator)
        {
            this.Value = Value;
            Query = BuildQuery();
        }
        //public static implicit operator SearchByName(SearchBy<bool> obj)
        //{
        //    return new SearchByName(obj.)
        //}
    }
}
