using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace TestDataSeeding.Model
{
    internal class EntityCatalogItem
    {
        [YAXAttributeForClass()]
        public string EntityName
        {
            get;
            set;
        }

        [YAXDictionary(EachPairName = "KeyValuePair", KeyName = "Key", ValueName = "Value",
                   SerializeKeyAs = YAXNodeTypes.Attribute,
                   SerializeValueAs = YAXNodeTypes.Attribute)]
        [YAXSerializeAs("PrimaryKeyValues")]
        public Dictionary<string, string> PrimaryKeyValues
        {
            get;
            set;
        }

        public string Filename
        {
            get;
            set;
        }

        public EntityCatalogItem()
        {
            EntityName = string.Empty;
            Filename = string.Empty;
            PrimaryKeyValues = new Dictionary<string, string>();
        }

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
