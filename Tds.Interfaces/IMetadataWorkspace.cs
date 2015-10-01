using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces.Metadata;

namespace Tds.Interfaces
{
    public interface IMetadataWorkspace
    {
        EntityType GetEntityType(string entityName);

        IEnumerable<Association> GetAssociationsWhereEntityIsPrincipal(string entityName, IEntityTypeFilter filter);

        IEnumerable<Association> GetAssociationsWhereEntityIsDependent(string entityName, IEntityTypeFilter filter);
    }
}
