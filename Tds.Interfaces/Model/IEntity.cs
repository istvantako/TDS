using System.Collections.Generic;

namespace Tds.Interfaces.Model
{
    public interface IEntity
    {
        string Name { get; set; }
        
        IDictionary<string, object> Properties { get; set; }
    }
}
