using Tds.Interaces.Structure;

namespace Tds.Interaces
{
    public interface IStructureProvider
    {
        EntityStructure GetEntityStructure(string entityName);
    }
}
