using System.Collections.Generic;

namespace Tds.Interaces.Database
{
    public class Entity
    {
        public string Name { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}
