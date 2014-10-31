using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;

namespace TestDataSeeding.Logic
{
    /// <summary>
    /// Data access object (DAO) for SQL databases.
    /// </summary>
    interface ISqlDataAccess
    {
        /// <summary>
        /// Saves the <paramref name="entity"/> to the database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void SaveEntity(Entity entity);

        /// <summary>
        /// Returns a new entity identified by <paramref name="entityName"/> and <paramref name="entityPrimaryKeyValues"/>.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="entityPrimaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        /// <returns>A new entity identified by <paramref name="entityName"/> and <paramref name="entityPrimaryKeyValues"/>.</returns>
        Entity GetEntity(string entityName, List<string> entityPrimaryKeyValues);
    }
}
