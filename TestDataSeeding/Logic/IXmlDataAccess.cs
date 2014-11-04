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
    public interface IXmlDataAccess
    {
        /// <summary>
        /// Saves the <paramref name="entity"/> to an XML file.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The structure of the entity.</param>
        void SaveEntity(Entity entity, EntityStructure entityStructure);

        /// <summary>
        /// Returns a new entity identified by the entity name from the <paramref name="entityStructure"/> and <paramref name="primaryKeyValues"/>.
        /// </summary>
        /// <param name="entityStructure">The structure of the entity.</param>
        /// <param name="primaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        /// <returns>A new entity identified by <paramref name="entityStructure"/> and <paramref name="primaryKeyValues"/>.</returns>
        Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues);

        /// <summary>
        /// Returns an EntityStructures collection.
        /// </summary>
        /// <returns>An EntityStructures collection.</returns>
        EntityStructures GetEntityStructures();
    }
}
