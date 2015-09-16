using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces.Metadata;
using YAXLib;

namespace Tds.StructureProviders.Xml
{
    [YAXSerializeAs("KeyMember")]
    public class XmlKeyMember : IKeyMember
    {
        [YAXAttributeForClass(), YAXSerializeAs("Order")]
        public int Sequence { get; set; }

        public string Name { get; set; }

        public XmlKeyMember()
        {
            Sequence = 0;
            Name = string.Empty;
        }
    }
}
