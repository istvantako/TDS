using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tds.Interfaces.Metadata
{
    public interface IAssociation
    {
        string Name { get; set; }

        string Principal { get; set; }

        string Dependent { get; set; }

        IDictionary<string, string> PropertyMappings { get; set; }
    }
}
