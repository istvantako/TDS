using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tds.Interfaces.Model
{
    public class EntityKey : IEntityKey
    {
        public string Name { get; set; }

        public object Value { get; set; }
    }
}
