using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;

namespace TestDataSeeding.Logic
{
    /// <summary>
    /// Interface for serialized object storage client.
    /// </summary>
    public interface ISerializedStorageClient
    {
        /// <summary>
        /// Saves the <paramref name="entity"/> to an XML file.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The structure of the entity.</param>
        /// <param name="path">The path where to save the entity.</param>
        void SaveEntity(Entity entity, EntityStructure entityStructure, string path);

        /// <summary>
        /// Returns a new entity identified by the entity name from the <paramref name="entityStructure"/> and <paramref name="primaryKeyValues"/>.
        /// </summary>
        /// <param name="entityStructure">The structure of the entity.</param>
        /// <param name="primaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        /// <param name="path">The path where the entity is stored.</param>
        /// <returns>A new entity identified by <paramref name="entityStructure"/> and <paramref name="primaryKeyValues"/>.</returns>
        Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues, string path);

        /// <summary>
        /// Returns true if the entity at the specified path has already been saved.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="primaryKeyValues">The list of primary key values.</param>
        /// <param name="path">The specified path.</param>
        /// <returns></returns>
        bool IsSaved(string entityName, List<string> primaryKeyValues, string path);

        /// <summary>
        /// Returns an EntityStructures collection from the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path where the entity structures are stored.</param>
        /// <returns>An EntityStructures collection from the specified <paramref name="path"/>.</returns>
        EntityStructures GetEntityStructures(string path);

        /// <summary>
        /// Saves the given <paramref name="entityStructures"/> collection in the given <paramref name="path"/>.
        /// </summary>
        /// <param name="entityStructures">The given EntityStructures collection.</param>
        /// <param name="path">The storage path where to save the collection.</param>
        void SaveEntityStructures(EntityStructures entityStructures, string path);
    }
}
