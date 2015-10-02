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

        public MetadataWorkspace()
        {
            EntityTypes = new List<EntityType>();
            Associations = new List<Association>();
        }

        public EntityType GetEntityType(string entityName)
        {
            TestEntityNameIsNull(entityName);

            try
            {
                return EntityTypes.Where(entityType => entityType.Name.Equals(entityName)).First();
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public IEnumerable<Association> GetAssociationsWhereEntityIsPrincipal(string entityName, IEntityTypeFilter filter)
        {
            TestEntityNameIsNull(entityName);

            return Associations.Where(association => association.Principal.Equals(entityName))
                               .Where(association => !filter.IsSkipped(association.Dependent));
        }

        public IEnumerable<Association> GetAssociationsWhereEntityIsDependent(string entityName, IEntityTypeFilter filter)
        {
            TestEntityNameIsNull(entityName);

            return Associations.Where(association => association.Dependent.Equals(entityName))
                               .Where(association => !filter.IsSkipped(association.Principal));
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
