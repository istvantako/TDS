using System.Collections.Generic;

using Tds.Interfaces.Database;
using Tds.Interfaces.Structure;

namespace Tds.Interfaces
{
    public interface IStorageProvider
    {
        Entity Read(string entityName, IEnumerable<EntityKey> keys, EntityStructure structure);
        void Write(Entity entity, IEnumerable<EntityKey> keys, EntityStructure structure);
    }
}
