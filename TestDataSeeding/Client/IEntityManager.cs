using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.Client
{
    /// <summary>
    /// Interface for managing the entities.
    /// </summary>
    public interface IEntityManager
    {
        /// <summary>
        /// Loads a saved entity, identified by <paramref name="entityName"/> and <paramref name="entityPrimaryKeyValues"/>,
        /// into the database, from the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="entityPrimaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        /// <param name="path">The path where the entity is stored.</param>
        void LoadEntity(string entityName, List<string> entityPrimaryKeyValues, string path);

        /// <summary>
        /// Saves an entity, identified by <paramref name="entityName"/> and <paramref name="entityPrimaryKeyValues"/>,
        /// to the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="entityPrimaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        /// <param name="path">The path where the entity will be stored.</param>
        void SaveEntity(string entityName, List<string> entityPrimaryKeyValues, string path);
    }
}
