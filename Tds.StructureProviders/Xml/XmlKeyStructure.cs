using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces.Structure;
using YAXLib;

namespace Tds.StructureProviders.Xml
{
    [YAXSerializeAs("Key")]
    public class XmlKeyStructure : IKeyStructure
    {
        [YAXAttributeForClass(), YAXSerializeAs("Order")]
        public override int Sequence { get; set; }

        public override string Name { get; set; }

        public XmlKeyStructure()
        {
            Name = string.Empty;
        }
    }
}
