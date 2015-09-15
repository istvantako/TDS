using System.Collections.Generic;

using Tds.Interfaces.Database;
using Tds.Interfaces.Structure;

namespace Tds.Interfaces
{
    public interface IStorageProvider
    {
        public virtual IStructureProvider StructureProvider { get; set; }

        IEntity Read(string entityName, IEnumerable<IEntityKey> keys);
        void Write(IEntity entity, IEnumerable<IEntityKey> keys);
    }
}
