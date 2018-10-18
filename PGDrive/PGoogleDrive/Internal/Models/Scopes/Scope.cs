using System.Collections.Generic;

namespace PGoogleDrive.Internal.Models.Scopes
{
    /// <summary>
    /// Represents an abstract class for containig list of google drive api scopes
    /// </summary>
    public abstract class PGScope
    {
        /// <summary>
        /// Initialize this PGScope with a string scope
        /// </summary>
        /// <param name="Scope"></param>
        public PGScope(string Scope)
        {
            Scopes = new List<string>() { Scope };
        }

        /// <summary>
        /// Contain all included into this object scopes
        /// </summary>
        public List<string> Scopes { get; set; }

        /// <summary>
        /// Adds a new scope to this object from andObject
        /// </summary>
        /// <param name="andObject"></param>
        /// <returns></returns>
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
