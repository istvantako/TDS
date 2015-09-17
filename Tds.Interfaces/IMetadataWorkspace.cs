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

        IEnumerable<Association> GetAssociationsWhereEntityIsPrincipal(string entityName);

        IEnumerable<Association> GetAssociationsWhereEntityIsDependent(string entityName);
    }
}
