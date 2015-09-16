using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces.Metadata;
using YAXLib;

namespace Tds.StructureProviders.Xml
{
    [YAXSerializeAs("Entity")]
    public class XmlEntityType : IEntityType
    {
        [YAXAttributeForClass(), YAXSerializeAs("Type")]
        public string Name { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive)]
        public IEnumerable<IKeyMember> PrimaryKey { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive)]
        public IEnumerable<IProperty> Properties { get; set; }

        public XmlEntityType()
        {
            Name = string.Empty;
            PrimaryKey = new List<IKeyMember>();
            Properties = new List<IProperty>();
        }
    }
}
