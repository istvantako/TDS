using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace TestDataSeeding.Model
{
    /// <summary>
    /// The catalog of the saved associative entities.
    /// </summary>
    internal class EntityCatalog
    {
        /// <summary>
        /// The list of catalog items (entities).
        /// </summary>
        [YAXDictionary(EachPairName = "Item")]
        [YAXSerializeAs("CatalogItems")]
        public List<EntityCatalogItem> EntityCatalogItems
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EntityCatalog()
        {
            EntityCatalogItems = new List<EntityCatalogItem>();
        }

        /// <summary>
        /// Adds a new catalog item (associative entity) to the catalog.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The structure of the entity.</param>
        /// <param name="filename">The name of the file.</param>
        public void Add(Entity entity, EntityStructure entityStructure, string filename)
        {
            if (!EntityCatalogItems.Exists(item => item.Filename.Equals(filename)))
            {
                EntityCatalogItems.Add(new EntityCatalogItem(entity, entityStructure, filename));
            }
        }

        /// <summary>
        /// Returns a list of partial matches with the given entities.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="keyValues">The key values pairs of the entity.</param>
        /// <returns>A list of partial matches with the given entities.</returns>
        public List<string> Find(string entityName, Dictionary<string, string> keyValues)
        {
            return EntityCatalogItems.Where(catalogItem => catalogItem.IsMatch(entityName, keyValues))
                                     .Select(catalogItem => catalogItem.Filename)
                                     .ToList();
        }
    }
}
