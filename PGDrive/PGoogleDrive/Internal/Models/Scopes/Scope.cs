using System.Collections.Generic;

namespace PGoogleDrive.Internal.Models.Scopes
{
    public abstract class PGScope
    {
        public PGScope(string Scope)
        {
            Scopes = new List<string>() { Scope };
        }

        public List<string> Scopes { get; set; }

        public PGScope And(PGScope andObject)
        {
            foreach(string scope in andObject.Scopes)
            {
                if (!Scopes.Contains(scope))
                {
                    Scopes.Add(scope);
                }
            }
            return this;
        }
    }
}
