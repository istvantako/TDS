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

        public List<Entity> GetAssociativeEntities(string entityName, Dictionary<string, string> keyValues)
        {
            List<Entity> queriedEntities = new List<Entity>();
            string query = queryBuilder.CreateSelectQuery(entityName, keyValues);
            try
            {
                SqlDataReader dataReader = queryExecutor.ExecuteQuery(query);

                while (dataReader.Read())
                {
                    Entity queriedEntity = new Entity();
                    queriedEntity.Name = entityName;

                    for (var i = 0; i < dataReader.FieldCount; i++)
                    {
                        queriedEntity.AttributeValues.Add(dataReader.GetName(i), dataReader[i].ToString());
                    }
                    queriedEntities.Add(queriedEntity);
                }

                dataReader.Close();
                queryExecutor.CloseConnection();

                return queriedEntities;
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
