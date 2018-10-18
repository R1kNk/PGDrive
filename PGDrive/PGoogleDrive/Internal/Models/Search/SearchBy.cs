namespace PGoogleDrive.Internal.Models.Search
{
    public abstract class SearchBy
    {

        public string Query { get; protected set; }

        protected string FieldName { get; set; }

        protected string Operator { get; set; }


        public SearchBy(string Field, string Operator) 
        {
            FieldName = Field;
            this.Operator = Operator;
        }

        abstract protected string BuildQuery();


        public SearchBy And(SearchBy andObject)
        {
            Query = Query.Insert(Query.Length - 1, " and "+andObject.Query);
            return this;
        }

        public SearchBy Or(SearchBy andObject)
        {
            Query = Query.Insert(Query.Length - 1, " or " + andObject.Query);
            return this;
        }

        public SearchBy Not(SearchBy andObject)
        {
            Query = Query.Insert(Query.Length - 1, " not " + andObject.Query);
            return this;
        }

    }
}
