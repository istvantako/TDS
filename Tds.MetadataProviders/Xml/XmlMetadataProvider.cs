using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces;
using Tds.Interfaces.Metadata;
using YAXLib;

namespace Tds.StructureProviders.Xml
{
    [YAXSerializeAs("EntityTypes")]
    public class XmlMetadataProvider : IMetadataProvider
    {
        [YAXCollection(YAXCollectionSerializationTypes.Recursive), YAXSerializeAs("Entities")]
        public IEnumerable<IEntityType> EntityTypes { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive)]
        public IEnumerable<IAssociation> Associations { get; set; }

        public XmlMetadataProvider()
        {
            EntityTypes = new List<IEntityType>();
            Associations = new List<IAssociation>();
        }

        public IEntityType GetEntityType(string entityName)
        {
            if (entityName == null)
            {
                throw new ArgumentNullException("The entity name object is null!");
            }

            return EntityTypes.Where(structure => structure.Name.Equals(entityName)).First();
        }


        public IEnumerable<IAssociation> GetAssociationsWhereEntityIsPrincipal(string entityName)
        {
            if (entityName == null)
            {
                throw new ArgumentNullException("The entity name object is null!");
            }

            return Associations.Where(association => association.Principal.Equals(entityName));
        }

        public IEnumerable<IAssociation> GetAssociationsWhereEntityIsDependent(string entityName)
        {
            if (entityName == null)
            {
                throw new ArgumentNullException("The entity name object is null!");
            }

            return Associations.Where(association => association.Dependent.Equals(entityName));
        }
    }
}
