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
    /// Database client interface.
    /// </summary>
    internal interface IDbClient
    {
        /// <summary>
        /// Returns a new entity identified by the entity name from the <paramref name="entityStructure"/> and <paramref name="primaryKeyValues"/>,
        /// or null, if there is no such entity.
        /// </summary>
        /// <param name="entityStructure">The structure of the entity.</param>
        /// <param name="primaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        /// <returns>A new entity identified by <paramref name="entityName"/> and <paramref name="primaryKeyValues"/>.</returns>
        Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues);

        /// <summary>
        /// Returns a list of associative entities identified by the given key values.
        /// </summary>
        /// <param name="entityStructure">The structure of associative entity name.</param>
        /// <param name="keyValues">The dictionary with the given keys and values.</param>
        /// <returns>A list of associative entities identified by the given key values.</returns>
        List<Entity> GetAssociativeEntities(EntityStructure entityStructure, Dictionary<string, string> keyValues);

        /// <summary>
        /// Add the <paramref name="entity"/> to be inserted on next ExecuteTransaction() call.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The structure of the entity.</param>
        void InsertWithTransaction(Entity entity, EntityStructure entityStructure);

        /// <summary>
        /// Add the <paramref name="entity"/> to be updated on next ExecuteTransaction() call.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The structure of the entity.</param>
        void UpdateWithTransaction(Entity entity, EntityStructure entityStructure);

        /// <summary>
        /// Restores all the Entities added with Insert- or UpdateWithTransaction() or none, if an operation fails.
        /// On successful transaction, clears the transaction data for a fresh new transaction.
        /// </summary>
        void ExecuteTransaction();

        /// <summary>
        /// Gets the structure of the database and returns it as an EntityStrucure object.
        /// </summary>
        /// <returns>The structure of the database as an EntityStructures object.</returns>
        EntityStructures GetDatabaseStructure();
    }
}
