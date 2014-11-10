using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TestDataSeeding.Logic;
using TestDataSeeding.Model;

namespace TestDataSeeding.SqlDataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private SqlQueryBuilder queryBuilder = new SqlQueryBuilder();
        private SqlQueryExecutor queryExecutor = new SqlQueryExecutor();

        public void SaveEntity(Entity entity, EntityStructure entityStructure)
        {
            string query = queryBuilder.CreateUpdateQuery(entity,entityStructure);

            int rowsAffected = queryExecutor.ExecuteNonQuery(query);
            if (rowsAffected == 0)
            {
                query = queryBuilder.CreateInsertQuery(entity, entityStructure);
                rowsAffected = queryExecutor.ExecuteNonQuery(query);
                if (rowsAffected == 0)
                {
                    throw (new SqlDataAccessException("Entity could not be inserted."));
                }
            }
            else
            {
                if (rowsAffected > 1)
                {
                    throw (new SqlDataAccessException("Multiple rows affected."));
                }
            }
        }

        public Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues)
        {
            string query = queryBuilder.CreateSelectQuery(entityStructure,primaryKeyValues);

            Entity queriedEntity;
            SqlDataReader dataReader = queryExecutor.ExecuteQuery(query);
            if (dataReader != null)
            {
                if (dataReader.Read())
                {
                    queriedEntity = new Entity(entityStructure.Name);
                    for (var i = 0; i < dataReader.FieldCount; i++)
                    {
                        queriedEntity.AttributeValues.Add(dataReader.GetName(i), dataReader[i].ToString());
                    }

                    if (dataReader.Read())
                    {
                        throw (new SqlDataAccessException("Multiple matches found."));
                    }
                }
                else
                {
                    throw (new SqlDataAccessException("No matches found."));
                }
                dataReader.Close();
            }
            else
            {
                throw (new SqlDataAccessException("Table or attribute name not found."));
            }

            return queriedEntity;
        }

        /// <summary>
        /// Constructs a new SqlDataAccess.
        /// </summary>
        /// <param name="connectionString">An SQL connection string.</param>
        public SqlDataAccess()
        {
            if (!queryExecutor.OpenConnection())
            {
                throw (new SqlDataAccessException("Invalid connection."));
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void ConnectionTeardown()
        {
            queryExecutor.CloseConnection();
        }
    }
}
