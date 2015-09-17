using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Tds.Interfaces.Metadata
{
    [YAXSerializeAs("Metadata")]
    public class MetadataWorkspace : IMetadataWorkspace
    {
        [YAXCollection(YAXCollectionSerializationTypes.Recursive), YAXSerializeAs("Entities")]
        public List<EntityType> EntityTypes { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive)]
        public List<Association> Associations { get; set; }

        public EntityType GetEntityType(string entityName)
        {
            TestEntityNameIsNull(entityName);

            return EntityTypes.Where(structure => structure.Name.Equals(entityName)).First();
        }

        public IEnumerable<Association> GetAssociationsWhereEntityIsPrincipal(string entityName)
        {
            TestEntityNameIsNull(entityName);

            return Associations.Where(association => association.Principal.Equals(entityName));
        }

        public IEnumerable<Association> GetAssociationsWhereEntityIsDependent(string entityName)
        {
            TestEntityNameIsNull(entityName);

            return Associations.Where(association => association.Dependent.Equals(entityName));
        }

        private void TestEntityNameIsNull(string entityName)
        {
            if (entityName == null)
            {
                throw new ArgumentNullException("The entity name object is null!");
            }
        }
    }
}
