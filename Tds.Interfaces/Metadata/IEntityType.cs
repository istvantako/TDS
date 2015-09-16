using System.Collections.Generic;
namespace Tds.Interfaces.Metadata
{
    public interface IEntityType
    {
        string Name { get; set; }

        IEnumerable<IKeyMember> PrimaryKey { get; set; }

        IEnumerable<IProperty> Properties { get; set; }
    }
}
