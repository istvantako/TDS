using System.Collections.Generic;

using Tds.Interfaces.Model;
using Tds.Interfaces.Metadata;

namespace Tds.Interfaces
{
    public interface IStorageProvider
    {
        IMetadataProvider MetadataProvider { get; set; }

        IEnumerable<Entity> Read(string entityName, IEnumerable<EntityKey> keys);

        void Write(Entity entity, IEnumerable<EntityKey> keys);
    }
}
