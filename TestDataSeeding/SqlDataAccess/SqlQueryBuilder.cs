using System;
using System.Collections.Generic;
using TestDataSeeding.Model;

namespace TestDataSeeding.SqlDataAccess
{
    internal class SqlQueryBuilder
    {
        /// <summary>
        /// Creates the select query to retrieve the given entity.
        /// </summary>
        /// <param name="entityStructure">An entity structure.</param>
        /// <param name="primaryKeyValues">An string list containing the primary keys.</param>
        /// <returns>Returns an SQL SELECT query string.</returns>
        public string CreateSelectQuery(EntityStructure entityStructure, List<string> primaryKeyValues)
        {
            string query = "SELECT * FROM " + entityStructure.Name + " WHERE ";

            for (var i = 0; i < entityStructure.PrimaryKeys.Count; i++)
            {
                query += entityStructure.PrimaryKeys[i] + "='" + primaryKeyValues[i] + "' and ";
            }

            query = query.Remove(query.Length - 5);

            return query;
        }

        /// <summary>
        /// Creates the update query which updates the given entity.
        /// </summary>
        /// <param name="entity">An entity.</param>
        /// <param name="entityStructure">The structure of the givem entity.</param>
        /// <returns>Returns an SQL UPDATE query string.</returns>
        public string CreateUpdateQuery(Entity entity, EntityStructure entityStructure)
        {
            string query = "UPDATE " + entityStructure.Name + " SET ";

            foreach (var entry in entity.AttributeValues)
            {
                if (!entityStructure.isPrimaryKey(entry.Key))
                {
                    query += entry.Key + "='" + entry.Value + "',";
                }
            }
            query = query.Remove(query.Length - 1);
            query += " WHERE ";

            for (var i = 0; i < entityStructure.PrimaryKeys.Count; i++)
            {
                query += entityStructure.PrimaryKeys[i] + "='" + entity.AttributeValues[entityStructure.PrimaryKeys[i]] + "' and ";
            }
            query = query.Remove(query.Length - 5);
            return query;
        }

        /// <summary>
        /// Creates the insert query to insert the given entity.
        /// </summary>
        /// <param name="entity">An entity.</param>
        /// <param name="entityStructure">The structure of the givem entity.</param>
        /// <returns>Returns an SQL INSERT query string.</returns>
        public string CreateInsertQuery(Entity entity, EntityStructure entityStructure)
        {
            string query = "INSERT INTO " + entity.Name + " Values (";
            foreach (var entry in entity.AttributeValues)
            {
                query += "'" + entry.Value + "',";
            }

            query = query.Remove(query.Length - 1);
            query += ")";

            return query;
        }
    }
}
