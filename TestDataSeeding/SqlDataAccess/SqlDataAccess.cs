using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Logic;
using TestDataSeeding.Model;

namespace TestDataSeeding.SqlDataAccess
{
    public class SqlDataAccess : Sql, ISqlDataAccess
    {
        public void SaveEntity(Entity entity, EntityStructure entityStructure)
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
                query += entityStructure.PrimaryKeys[i] + "='" + entity.AttributeValues[entityStructure.PrimaryKeys[i]] + "'";
                if (i < entityStructure.PrimaryKeys.Count - 1)
                {
                    query += " and ";
                }
            }

            Debug.WriteLine(query);

            Debug.WriteLine(ExecuteNonQuery(query));
        }

        public Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues)
        {
            string error = string.Empty;
            string query = "SELECT * FROM " + entityStructure.Name + " WHERE ";
            for (var i = 0; i < entityStructure.PrimaryKeys.Count; i++)
            {
                query += entityStructure.PrimaryKeys[i] + "='" + primaryKeyValues[i] + "'";
                if (i < entityStructure.PrimaryKeys.Count - 1)
                {
                    query += " and ";
                }
            }
          
            SqlDataReader dataReader = ExecuteReader(query, ref error);
            if (dataReader != null)
            {
                Entity queriedEntity = new Entity(entityStructure.Name);
                
                while (dataReader.Read())
                {
                    for (var i = 0; i < dataReader.FieldCount; i++)
                    {
                        queriedEntity.AttributeValues.Add(dataReader.GetName(i), dataReader[i].ToString());
                    }
                }

                CloseDataReader(dataReader);
                return queriedEntity;
            }
            else
            {
                CloseDataReader(dataReader);
                return null;
            }
        }

        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void SetSqlConnection(System.Data.SqlClient.SqlConnection sqlConnection)
        {
            Connection = sqlConnection;
        }
    }
}
