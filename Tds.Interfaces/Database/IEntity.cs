using System.Collections.Generic;

namespace Tds.Interfaces.Database
{
    public interface IEntity
    {
        public virtual string Name { get; set; }
        public virtual Dictionary<string, object> Properties { get; set; }
    }
}
