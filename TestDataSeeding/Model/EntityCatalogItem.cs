using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace TestDataSeeding.Model
{
    /// <summary>
    /// Associative entity catalog item.
    /// </summary>
    internal class EntityCatalogItem
    {
        /// <summary>
        /// The name of the associative entity.
        /// </summary>
        [YAXAttributeForClass()]
        public string EntityName
        {
            get;
            set;
        }

        /// <summary>
        /// The dictionary of the primary key names and their values.
        /// </summary>
        [YAXDictionary(EachPairName = "KeyValuePair", KeyName = "Key", ValueName = "Value",
                   SerializeKeyAs = YAXNodeTypes.Attribute,
                   SerializeValueAs = YAXNodeTypes.Attribute)]
        [YAXSerializeAs("PrimaryKeyValues")]
        public Dictionary<string, string> PrimaryKeyValues
        {
            get;
            set;
        }

        /// <summary>
        /// The file name of the associative entity.
        /// </summary>
        public string Filename
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EntityCatalogItem()
        {
            EntityName = string.Empty;
            Filename = string.Empty;
            PrimaryKeyValues = new Dictionary<string, string>();
        }

        /// <summary>
        /// Constructs a new catalog item with the given values.
        /// </summary>
        /// <param name="entity">The name of the associative entity.</param>
        /// <param name="entityStructure">The structure of the entity.</param>
        /// <param name="filename">The name of the file.</param>
        public EntityCatalogItem(Entity entity, EntityStructure entityStructure, string filename)
        {
            EntityName = entity.Name;
            Filename = filename;
            PrimaryKeyValues = new Dictionary<string, string>();

            foreach (var key in entityStructure.PrimaryKeys)
            {
                PrimaryKeyValues.Add(key, entity.AttributeValues[key]);
            }
        }

        /// <summary>
        /// Returns true, if the given entity partially matches the current entity (catalog item).
        /// </summary>
        /// <param name="entityName">The given entity name.</param>
        /// <param name="keyValues">The given key and value pairs.</param>
        /// <returns>True, if the given entity partially matches the current entity (catalog item).</returns>
        public bool IsMatch(string entityName, Dictionary<string, string> keyValues)
        {
            if (!EntityName.Equals(entityName))
            {
                return false;
            }

            foreach (var keyValue in keyValues)
            {
                if (!PrimaryKeyValues.Contains(keyValue))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
