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
    internal interface ISerializedStorageClient
    {
        /// <summary>
        /// Prepares a transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Executes the prepared transaction.
        /// </summary>
        void ExecuteTransaction();

        /// <summary>
        /// Serializes the <paramref name="entity"/> and saves it in a file.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The structure of the entity.</param>
        /// <param name="path">The path where to save the entity.</param>
        void SaveEntity(Entity entity, EntityStructure entityStructure, string path);

        /// <summary>
        /// Returns a new entity identified by the entity name from the <paramref name="entityStructure"/> and
        /// <paramref name="primaryKeyValues"/>.
        /// </summary>
        /// <param name="entityStructure">The structure of the entity.</param>
        /// <param name="primaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        /// <param name="path">The path where the entity is stored.</param>
        /// <returns>A new entity identified by <paramref name="entityStructure"/> and <paramref name="primaryKeyValues"/>.</returns>
        Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues, string path);

        /// <summary>
        /// Returns the entity saved in the given file.
        /// </summary>
        /// <param name="filename">The path to the file.</param>
        /// <returns>The entity saved in the given file.</returns>
        Entity GetEntity(string filename);

        /// <summary>
        /// Returns true if the entity at the specified path has already been saved.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="primaryKeyValues">The list of primary key values.</param>
        /// <param name="path">The specified path.</param>
        /// <returns>True if the entity at the specified path has already been saved.</returns>
        bool IsSaved(string entityName, List<string> primaryKeyValues, string path);

        /// <summary>
        /// Returns the file name of the entity.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="primaryKeyValues">The list of primary key values.</param>
        /// <param name="path">The specified path.</param>
        /// <returns>The file name of the entity.</returns>
        string GetEntityFileName(string entityName, List<string> primaryKeyValues, string path);

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

        /// <summary>
        /// Returns the catalog with the associative entities from the given path.
        /// </summary>
        /// <param name="path">The given path.</param>
        /// <returns>The catalog with the associative entities from the given path.</returns>
        EntityCatalog GetCatalog(string path);

        /// <summary>
        /// Saves the catalog with the associative entities to the given path.
        /// </summary>
        /// <param name="catalog">The given catalog.</param>
        /// <param name="path">The given path.</param>
        void SaveCatalog(EntityCatalog catalog, string path);
    }
}
