using System;
using System.Collections.Generic;
using System.Linq;
using Tds.Interfaces.Metadata;
using Tds.Interfaces.Model;
using Tds.Types;

namespace Tds.Engine.Exceptions
{
    public class EntityNotFoundInDatabaseException : Exception
    {
        public EntityNotFoundInDatabaseException(string entityName, IEnumerable<EntityKey> keys, EntityType entityType)
            : base(string.Format("Entity '{0}' with keys [{1}] not found in the database.",
                    entityName, 
                    string.Join(", ", keys.Select(x => string.Format("{0}: {1}", x.Name, Converter.ConvertToString(entityType.Properties[x.Name], x.Value)).ToArray()))))
        { }
    }
}
