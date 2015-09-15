using System.Collections.Generic;
namespace Tds.Interfaces.Structure
{
    public interface IEntityStructure
    {
        public virtual string Type { get; set; }

        public virtual IEnumerable<IKeyStructure> Keys { get; set; }

        public virtual IEnumerable<IPropertyStructure> Properties { get; set; }

        public virtual IEnumerable<IDependencyStructure> Dependencies { get; set; }
    }
}
