using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Types;

namespace Tds.Interfaces.Structure
{
    public interface IPropertyStructure
    {
        public virtual string Name { get; set; }

        public virtual DataType Type { get; set; }
    }
}
