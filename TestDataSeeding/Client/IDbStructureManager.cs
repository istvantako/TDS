using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;

namespace TestDataSeeding.Client
{
    /// <summary>
    /// Interface for retrieving the database structure.
    /// </summary>
    internal interface IDbStructureManager
    {
        /// <summary>
        /// Returns the database structure as entity structures.
        /// </summary>
        /// <returns>The database structure as entity structures.</returns>
        EntityStructures GetDatabaseStructure();
    }
}
