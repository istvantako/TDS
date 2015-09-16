using System.Collections.Generic;

using Tds.Interfaces.Model;
using Tds.Interfaces.Metadata;

namespace Tds.Interfaces
{
    public interface IStorageProvider
    {
        IMetadataProvider MetadataProvider { get; set; }

        IEnumerable<IEntity> Read(string entityName, IEnumerable<IEntityKey> keys);

        void Write(IEntity entity, IEnumerable<IEntityKey> keys);
    }
}
