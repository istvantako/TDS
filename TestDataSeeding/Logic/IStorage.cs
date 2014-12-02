using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestDataSeeding.Model;

namespace TestDataSeeding.Logic
{
    public interface  IStorage
    {
        /// <summary>
        /// Returns an EntityStructures collection from the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path where the entity structures are stored.</param>
        /// <returns>An EntityStructures collection from the specified <paramref name="path"/>.</returns>
        EntityStructures GetEntityStructures(string path);
    }
}
