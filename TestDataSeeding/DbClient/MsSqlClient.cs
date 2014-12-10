using System.Collections.Generic;
using System.Data.SqlClient;
using TestDataSeeding.Client;
using TestDataSeeding.Logic;
using TestDataSeeding.Model;
using System.Text;
using System;
using System.Diagnostics;

namespace TestDataSeeding.DbClient
{
    internal class MsSqlClient : IDbClient, IDbStructureManager
    {
        private MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder();
        private MsSqlQueryExecutor queryExecutor = new MsSqlQueryExecutor();
        private MsSqlStructureBuilder structureBuilder = new MsSqlStructureBuilder();
        private List<string> transactionData = new List<string>();

        public Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues)
        {
            Entity queriedEntity = new Entity();
            string query = queryBuilder.CreateSelectQuery(entityStructure, primaryKeyValues);
            try
            {
                SqlDataReader dataReader = queryExecutor.ExecuteQuery(query);
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
            }
            catch (DbException)
            {
                throw;
            }

            ConvertData(ref queriedEntity, entityStructure);
            return queriedEntity;
        }

        public List<Entity> GetAssociativeEntities(EntityStructure entityStructure, Dictionary<string, string> keyValues)
        {
            List<Entity> queriedEntities = new List<Entity>();
            string query = queryBuilder.CreateSelectQuery(entityStructure.Name, keyValues);
            try
            {
                SqlDataReader dataReader = queryExecutor.ExecuteQuery(query);

                while (dataReader.Read())
                {
                    Entity queriedEntity = new Entity();
                    queriedEntity.Name = entityStructure.Name;

                    for (var i = 0; i < dataReader.FieldCount; i++)
                    {
                        queriedEntity.AttributeValues.Add(dataReader.GetName(i), dataReader[i].ToString());
                    }

                    ConvertData(ref queriedEntity, entityStructure);
                    queriedEntities.Add(queriedEntity);
                }

                dataReader.Close();
                queryExecutor.CloseConnection();
            }
            catch (DbException)
            {
                throw;
            }

            return queriedEntities;
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

        public EntityStructures GetDatabaseStructure()
        {
            EntityStructures structures = new EntityStructures();

            try
            {
                foreach (var tableName in structureBuilder.GetTablesNames())
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(tableName.Item1).Append(".").Append(tableName.Item2);

                    EntityStructure structure = new EntityStructure(builder.ToString());
                    structureBuilder.SetTableAttributes(ref structure, tableName.Item1, tableName.Item2);
                    structureBuilder.SetTablePrimaryKeys(ref structure, tableName.Item1, tableName.Item2);
                    structureBuilder.SetTableForeignKeys(ref structure, tableName.Item1, tableName.Item2);
                    structures.Add(structure);
                }
                //if a table has references to more than one table, it is considered a relationship table
                //we set the BelongsToMany fields in the referenced Tables
                foreach (var structure in structures.Structures)
                {
                    //use hashset to escape multiple references to same table
                    HashSet<string> referrencedTables = new HashSet<string>();
                    foreach (var foreignKey in structure.ForeignKeys)
                    {
                        if (structure.IsPrimaryKey(foreignKey.Key))
                        {
                            referrencedTables.Add(foreignKey.Value.EntityName);
                        }
                    }

                    if (referrencedTables.Count > 1)
                    {
                        foreach (var table in referrencedTables)
                        {
                            structures.Find(table).BelongsToMany.Add(structure.Name);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return structures;
        }

        /// <summary>
        /// Converts some attributes of the referenced <paramref name="entity"/> to be suitable for
        /// loading them back in the database
        /// </summary>
        /// <param name="entity">An Entity reference.</param>
        /// <param name="entity">Structure of the <paramref name="entity"/>.</param>
        private void ConvertData(ref Entity entity, EntityStructure structure)
        {
            foreach (var attribute in structure.Attributes)
            {
                if (attribute.Value.Equals("date"))
                {
                    if (entity.AttributeValues[attribute.Key] != "")
                    {
                        DateTime dt = Convert.ToDateTime(entity.AttributeValues[attribute.Key]);
                        entity.AttributeValues[attribute.Key] = dt.ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz");
                    }
                }
            }
        }
    }
}
