using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;

namespace TestDataSeeding.Client
{
    /// <summary>
    /// Interface for saving and retrieving the entity structures from the disk.
    /// </summary>
    internal interface ISerializedStorageStructureManager
    {
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
