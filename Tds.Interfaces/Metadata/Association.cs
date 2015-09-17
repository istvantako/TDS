using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Tds.Interfaces.Metadata
{
    public class Association
    {
        [YAXAttributeForClass()]
        public string Name { get; set; }

        public string Principal { get; set; }

        public string Dependent { get; set; }

        [YAXDictionary(EachPairName = "PropertyRef", KeyName = "OnPrincipal", ValueName = "OnDependent",
                SerializeKeyAs = YAXNodeTypes.Attribute,
                SerializeValueAs = YAXNodeTypes.Attribute)]
        public Dictionary<string, string> PropertyMappings { get; set; }

        public Association()
        {
            Name = string.Empty;
            Principal = string.Empty;
            Dependent = string.Empty;
            PropertyMappings = new Dictionary<string, string>();
        }
    }
}
