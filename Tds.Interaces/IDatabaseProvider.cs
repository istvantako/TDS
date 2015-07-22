using System.Collections.Generic;

using Tds.Interaces.Database;

namespace Tds.Interaces
{
    public interface IDatabaseProvider
    {
        List<Entity> GetEntities(string entityName, params EntityKey[] keys);
    }
}
