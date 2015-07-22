using System;
using System.Collections.Generic;
using System.Linq;

namespace Tds.Engine.Exceptions
{
    public class EntityNotFoundInDatabaseException : Exception
    {
        public EntityNotFoundInDatabaseException(string entityName, Dictionary<string, string> keys)
            : base(string.Format("Entity '{0}' with keys [{1}] not found in the database.",
                    entityName, 
                    string.Join(", ", keys.Select(x => string.Format("{0}: {1}", x.Key, x.Value)).ToArray())))
        { }
    }
}
