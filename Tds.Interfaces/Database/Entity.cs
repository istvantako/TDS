using System.Collections.Generic;

namespace Tds.Interfaces.Database
{
    public class Entity
    {
        public string Name { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}
