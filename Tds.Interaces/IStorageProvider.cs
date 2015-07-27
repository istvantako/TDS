using System.Collections.Generic;

using Tds.Interaces.Database;
using Tds.Interaces.Structure;

namespace Tds.Interaces
{
    public interface IStorageProvider
    {
        Entity Read(string entityName, IEnumerable<EntityKey> keys, EntityStructure structure);
        void Write(Entity entity, IEnumerable<EntityKey> keys, EntityStructure structure);
    }
}
