using System.Collections.Generic;
using Tds.Interfaces.Metadata;

namespace Tds.Interfaces
{
    public interface IMetadataProvider
    {
        IEnumerable<IEntityType> EntityTypes { get; set; }

        IEnumerable<IAssociation> Associations { get; set; }

        IEntityType GetEntityType(string entityName);

        IEnumerable<IAssociation> GetAssociationsWhereEntityIsPrincipal(string entityName);

        IEnumerable<IAssociation> GetAssociationsWhereEntityIsDependent(string entityName);
    }
}
