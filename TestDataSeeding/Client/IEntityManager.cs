using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;

namespace TestDataSeeding.Client
{
    /// <summary>
    /// Interface for managing the entities.
    /// </summary>
    internal interface IEntityManager
    {
        /// <summary>
        /// Loads the given entities and their dependencies.
        /// </summary>
        /// <param name="entities">The list of entities with their primary key values.</param>
        /// <param name="path">The path where the entities are stored.</param>
        /// <param name="overwrite">If true, overwrite the already saved entities.</param>
        void LoadEntities(List<EntityWithKey> entities, string path, bool overwrite = false);

        /// <summary>
        /// Saves the given entities and their dependencies.
        /// </summary>
        /// <param name="entities">The list of entities with their primary key values.</param>
        /// <param name="path">The path where the entities are stored.</param>
        /// <param name="overwrite">If true, overwrite the already saved entities.</param>
        void SaveEntities(List<EntityWithKey> entities, string path, bool overwrite = false);
    }
}
