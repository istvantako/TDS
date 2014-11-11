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
            string query = queryBuilder.CreateUpdateQuery(entity, entityStructure);
            try
            {
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
                else if (rowsAffected > 1)
                {
                    throw (new SqlDataAccessException("Multiple rows affected."));
                }
            }
            catch (SqlDataAccessException)
            {
                throw;
            }
        }

        public Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues)
        {
            string query = queryBuilder.CreateSelectQuery(entityStructure, primaryKeyValues);
            try
            {
                SqlDataReader dataReader = queryExecutor.ExecuteQuery(query);
                Entity queriedEntity = new Entity(entityStructure.Name);

                if (dataReader.Read())
                {
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
                queryExecutor.CloseConnection();

                return queriedEntity;
            }
            catch (SqlDataAccessException)
            {
                throw;
            }
        }
    }
}
