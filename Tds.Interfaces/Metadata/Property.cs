using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Types;
using YAXLib;

namespace Tds.Interfaces.Metadata
{
    public class Property
    {
        [YAXAttributeForClass()]
        public string Name { get; set; }

        [YAXAttributeForClass()]
        public DataType Type { get; set; }

        public Property()
        {
            Name = string.Empty;
            Type = DataType.Undefined;
        }

        public Property(string name, DataType type)
        {
            Name = name;
            Type = type;
        }
    }
}
