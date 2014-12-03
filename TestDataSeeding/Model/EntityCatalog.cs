using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace TestDataSeeding.Model
{
    internal class EntityCatalog
    {
        [YAXDictionary(EachPairName = "Item")]
        [YAXSerializeAs("CatalogItems")]
        public List<EntityCatalogItem> EntityCatalogItems
        {
            get;
            set;
        }

        public EntityCatalog()
        {
            EntityCatalogItems = new List<EntityCatalogItem>();
        }

        public void Add(Entity entity, EntityStructure entityStructure, string filename)
        {
            if (!EntityCatalogItems.Exists(item => item.Filename.Equals(filename)))
            {
                EntityCatalogItems.Add(new EntityCatalogItem(entity, entityStructure, filename));
            }
        }
    }
}
