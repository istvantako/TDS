using System.Collections.Generic;
using Tds.Types;
using YAXLib;
namespace Tds.Interfaces.Metadata
{
    [YAXSerializeAs("Entity")]
    public class EntityType
    {
        [YAXAttributeForClass(), YAXSerializeAs("Type")]
        public string Name { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive)]
        public List<KeyMember> PrimaryKey { get; set; }

        [YAXDictionary(EachPairName = "Property", KeyName = "Name", ValueName = "Type",
                SerializeKeyAs = YAXNodeTypes.Attribute,
                SerializeValueAs = YAXNodeTypes.Attribute)]
        public Dictionary<string, DataType> Properties { get; set; }

        public EntityType()
        {
            Name = string.Empty;
            PrimaryKey = new List<KeyMember>();
            Properties = new Dictionary<string, DataType>();
        }
    }
}
