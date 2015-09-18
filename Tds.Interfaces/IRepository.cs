using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces.Model;

namespace Tds.Interfaces
{
    public interface IRepository
    {
        IMetadataWorkspace MetadataWorkspace { get; set; }

        IEnumerable<Entity> Read(string entityName, IEnumerable<EntityKey> keys);

        void Write(Entity entity, IEnumerable<EntityKey> keys, EntityStatus status = EntityStatus.Added);
    }
}
