using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;

namespace TestDataSeeding.Logic
{
    /// <summary>
    /// Data access object (DAO) for XML files.
    /// </summary>
    interface IXmlDataAccess
    {
        /// <summary>
        /// Saves the <paramref name="entity"/> to an XML file.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void SaveEntity(Entity entity);

        /// <summary>
        /// Returns a new entity identified by <paramref name="entityName"/> and <paramref name="primaryKeyValues"/>.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="primaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        /// <returns>A new entity identified by <paramref name="entityName"/> and <paramref name="primaryKeyValues"/>.</returns>
        Entity GetEntity(string entityName, List<string> primaryKeyValues);

        /// <summary>
        /// Returns an EntityStructures collection.
        /// </summary>
        /// <returns>An EntityStructures collection.</returns>
        EntityStructures GetEntityStructures();
    }
}
