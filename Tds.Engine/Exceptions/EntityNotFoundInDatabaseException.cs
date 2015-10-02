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
        public EntityNotFoundInDatabaseException(string entityName, IEnumerable<EntityKey> keys, EntityType entityType, Exception innerException)
            : base (FormatMessage(entityName, keys, entityType), innerException)
        {
        }

        private static string FormatMessage(string entityName, IEnumerable<EntityKey> keys, EntityType entityType)
        {
            int index = 0;
            var formatKeyMembers = new string[keys.Count()];
            foreach (var key in keys)
            {
                formatKeyMembers[index] = string.Format("{0}: {1}", key.Name, Converter.ConvertToString(entityType.Properties[key.Name], key.Value));
                index++;
            }

            string message = string.Format("Entity '{0}' with keys [{1}] not found in the database.", entityName, string.Join(", ", formatKeyMembers));

            return message;
        }
    }
}
