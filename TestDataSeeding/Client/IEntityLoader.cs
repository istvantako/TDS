using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.Client
{
    /// <summary>
    /// Interface for providing a load option for an entity.
    /// </summary>
    interface IEntityLoader
    {
        /// <summary>
        /// Loads a saved entity, identified by <paramref name="entityName"/> and <paramref name="entityPrimaryKeyValues"/>,
        /// into the database.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="entityPrimaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        void LoadEntity(string entityName, List<string> entityPrimaryKeyValues);
    }
}
