using System.Collections.Generic;
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

        [YAXCollection(YAXCollectionSerializationTypes.Recursive)]
        public List<Property> Properties { get; set; }

        public EntityType()
        {
            Name = string.Empty;
            PrimaryKey = new List<KeyMember>();
            Properties = new List<Property>();
        }
    }
}
