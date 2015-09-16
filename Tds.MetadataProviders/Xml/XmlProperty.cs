using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces.Metadata;
using Tds.Types;
using YAXLib;

namespace Tds.StructureProviders.Xml
{
    [YAXSerializeAs("Property")]
    public class XmlProperty : IProperty
    {
        public string Name { get; set; }

        [YAXAttributeForClass(), YAXSerializeAs("Type")]
        public DataType Type { get; set; }

        public XmlProperty()
        {
            Name = string.Empty;
            Type = DataType.Undefined;
        }
    }
}
