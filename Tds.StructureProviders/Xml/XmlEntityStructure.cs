using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces.Structure;
using YAXLib;

namespace Tds.StructureProviders.Xml
{
    [YAXSerializeAs("Entity")]
    public class XmlEntityStructure : IEntityStructure
    {
        [YAXAttributeForClass(), YAXSerializeAs("Type")]
        public override string Type { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive), YAXSerializeAs("PrimaryKeys")]
        public override IEnumerable<IKeyStructure> Keys { get; set; }

        public XmlEntityStructure()
        {
            Type = string.Empty;
        }
    }
}
