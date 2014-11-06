using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;

namespace TestDataSeeding.Logic
{
    /// <summary>
    /// Data access object (DAO) for SQL databases.
    /// </summary>
    public interface ISqlDataAccess
    {
        /// <summary>
        /// Saves the <paramref name="entity"/> to the database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The structure of the entity.</param>
        void SaveEntity(Entity entity, EntityStructure entityStructure);

        /// <summary>
        /// Returns a new entity identified by the entity name from the <paramref name="entityStructure"/> and <paramref name="primaryKeyValues"/>.
        /// </summary>
        /// <param name="entityStructure">The structure of the entity.</param>
        /// <param name="primaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        /// <returns>A new entity identified by <paramref name="entityName"/> and <paramref name="primaryKeyValues"/>.</returns>
        Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues);

        /// <summary>
        /// Sets the connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        void SetConnectionString(string connectionString);

        /// <summary>
        /// Sets the SqlConnection for the database.
        /// </summary>
        /// <param name="sqlConnection">The SqlConnection object.</param>
        void SetSqlConnection(SqlConnection sqlConnection);

        /// <summary>
        /// Returns the SQL status of the last performed action.
        /// </summary>
        /// <returns>The SQL status of the last performed action.</returns>
        SqlStatus getSqlStatus();
    }
}
