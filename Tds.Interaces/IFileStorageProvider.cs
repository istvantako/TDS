using System.Collections.Generic;

using Tds.Interaces.Database;
using Tds.Interaces.Structure;

namespace Tds.Interaces
{
    public interface IFileStorageProvider
    {
        void Save(Entity entity, List<EntityKey> keys, EntityStructure structure);
        Entity Get(string name, List<object> keys);
    }
}
