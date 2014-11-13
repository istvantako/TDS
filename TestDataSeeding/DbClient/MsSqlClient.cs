using System.Collections.Generic;
using System.Data.SqlClient;
using TestDataSeeding.Logic;
using TestDataSeeding.Model;

namespace TestDataSeeding.DbClient
{
    public class MsSqlClient : IDbClient
    {
        private MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder();
        private MsSqlQueryExecutor queryExecutor = new MsSqlQueryExecutor();
        private List<string> transactionData = new List<string>();

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
                        throw (new DbException("Entity could not be inserted."));
                    }
                }
                else if (rowsAffected > 1)
                {
                    throw (new DbException("Multiple rows affected."));
                }
            }
            catch (DbException)
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
                Entity queriedEntity = new Entity();
                queriedEntity.Name = entityStructure.Name;

                if (dataReader.Read())
                {
                    for (var i = 0; i < dataReader.FieldCount; i++)
                    {
                        queriedEntity.AttributeValues.Add(dataReader.GetName(i), dataReader[i].ToString());
                    }

                    if (dataReader.Read())
                    {
                        throw (new DbException("Multiple matches found."));
                    }
                }
                else
                {
                    queriedEntity = null;
                }
                dataReader.Close();
                queryExecutor.CloseConnection();

                return queriedEntity;
            }
            catch (DbException)
            {
                throw;
            }
        }

        public void InsertWithTransaction(Entity entity, EntityStructure entityStructure)
        {
            transactionData.Add(queryBuilder.CreateInsertQuery(entity, entityStructure));
        }

        public void UpdateWithTransaction(Entity entity, EntityStructure entityStructure)
        {
            transactionData.Add(queryBuilder.CreateUpdateQuery(entity, entityStructure));
        }

        public void ExecuteTransaction()
        {
            try
            {
                queryExecutor.ExecuteTransaction(transactionData);
                transactionData.Clear();
            }
            catch
            {
                throw;
            }
        }
    }
}
