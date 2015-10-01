using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces;
using Tds.Interfaces.Metadata;
using Tds.Interfaces.Model;

namespace Tds.Interfaces
{
    /// <summary>
    /// This interface is implemented by any object that can resolve a dependency,
    /// either directly or through use of an external container.
    /// </summary>
    public interface IDependencyResolver
    {
        IRepository SourceRepository { get; set; }

        IEnumerable<Entity> GetEntitiesWhereEntityIsDependent(Entity entity, Association association);

        IEnumerable<Entity> GetEntitiesWhereEntityIsPrincipal(Entity entity, Association association);
    }
}
