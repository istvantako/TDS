using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces.Structure;
using Tds.Types;
using YAXLib;

namespace Tds.StructureProviders.Xml
{
    [YAXSerializeAs("Property")]
    public class XmlPropertyStructure : IPropertyStructure
    {
        public override string Name { get; set; }

        [YAXAttributeForClass(), YAXSerializeAs("Type")]
        public override DataType Type { get; set; }

        public XmlPropertyStructure()
        {
            Name = string.Empty;
            Type = DataType.Undefined;
        }
    }
}
