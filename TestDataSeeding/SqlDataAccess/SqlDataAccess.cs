using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TestDataSeeding.Logic;
using TestDataSeeding.Model;

namespace TestDataSeeding.SqlDataAccess
{
    public class SqlDataAccess : Sql, ISqlDataAccess
    {
        private SqlStatus status;

        public void SaveEntity(Entity entity, EntityStructure entityStructure)
        {
            status = SqlStatus.Success;

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
                query += entityStructure.PrimaryKeys[i] + "='" + entity.AttributeValues[entityStructure.PrimaryKeys[i]] + "'";
                if (i < entityStructure.PrimaryKeys.Count - 1)
                {
                    query += " and ";
                }
            }

            int rowsAffected = ExecuteNonQuery(query);

            if (rowsAffected == 0)
            {
                query = "INSERT INTO " + entity.Name + " Values (";
                foreach (var entry in entity.AttributeValues)
                {
                    query += "'" + entry.Value + "',";
                }

                query = query.Remove(query.Length - 1);
                query += ")";
            }
        }

        public Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues)
        {
            status = SqlStatus.Success;

            string query = "SELECT * FROM " + entityStructure.Name + " WHERE ";
            for (var i = 0; i < entityStructure.PrimaryKeys.Count; i++)
            {
                query += entityStructure.PrimaryKeys[i] + "='" + primaryKeyValues[i] + "' and ";
            }
            query = query.Remove(query.Length - 5);

            Entity queriedEntity;
            SqlDataReader dataReader = ExecuteQuery(query);

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
                        queriedEntity = null;
                        status = SqlStatus.MultipleMatchesFound;
                    }
                }
                else
                {
                    queriedEntity = null;
                    status = SqlStatus.NoMatchesFound;
                }
                dataReader.Close();
            }
            else
            {
                queriedEntity = null;
                status = SqlStatus.TableOrAttributeNameNotFound;
            }

            return queriedEntity;
        }

        public void SetConnectionString(string connectionString)
        {
            status = SqlStatus.Success;
            if (!OpenConnectionWithString(connectionString))
            {
                status = SqlStatus.InvalidConnectionString;
            }
        }

        public void SetSqlConnection(SqlConnection sqlConnection)
        {
            status = SqlStatus.Success;
            if (!OpenConnection(sqlConnection))
            {
                status = SqlStatus.InvalidConnection;
            }
        }

        public SqlStatus getSqlStatus()
        {
            return status;
        }

        /// <summary>
        /// Constructs a new SqlDataAccess.
        /// </summary>
        public SqlDataAccess() { }

        /// <summary>
        /// Constructs a new SqlDataAccess.
        /// </summary>
        /// <param name="connectionString">An SQL connection string.</param>
        public SqlDataAccess(string connectionString)
        {
            SetConnectionString(connectionString);
        }

        /// <summary>
        /// Constructs a new SqlDataAccess.
        /// </summary>
        /// <param name="sqlConnection">An sqlConnection object.</param>
        public SqlDataAccess(SqlConnection sqlConnection)
        {
            SetSqlConnection(sqlConnection);
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void ConnectionTeardown()
        {
            base.CloseConnection();
        }
    }
}
