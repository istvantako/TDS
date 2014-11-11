using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.Client
{
    /// <summary>
    /// Interface for providing a save option for an entity.
    /// </summary>
    interface IEntitySaver
    {
        /// <summary>
        /// Saves an entity, identified by <paramref name="entityName"/> and <paramref name="entityPrimaryKeyValues"/>.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="entityPrimaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        void SaveEntity(string entityName, List<string> entityPrimaryKeyValues, string path);

        /// <summary>
        /// Saves an entity, identified by <paramref name="entityName"/> and <paramref name="entityPrimaryKeyValues"/>.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="entityPrimaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        void SaveEntity(string entityName, List<string> entityPrimaryKeyValues);
    }
}
