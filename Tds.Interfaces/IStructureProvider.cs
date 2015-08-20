using Tds.Interfaces.Structure;

namespace Tds.Interfaces
{
    public interface IStructureProvider
    {
        EntityStructure GetEntityStructure(string entityName);
    }
}
