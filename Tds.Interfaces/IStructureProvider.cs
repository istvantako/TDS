using System.Collections.Generic;
using Tds.Interfaces.Structure;

namespace Tds.Interfaces
{
    public interface IStructureProvider
    {
        public virtual IEnumerable<IEntityStructure> EntityStructures { get; set; }

        IEntityStructure GetEntityStructure(string entityName);
    }
}
