using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Types;

namespace Tds.Interfaces.Metadata
{
    public interface IProperty
    {
        string Name { get; set; }

        DataType Type { get; set; }
    }
}
