using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;

namespace TestDataSeeding.DbClient
{
    internal class MsSqlStructureBuilder
    {
        private MsSqlQueryExecutor queryExecutor = new MsSqlQueryExecutor();

        /// <summary>
        /// Gets the name of every table in the database.
        /// </summary>
        /// <returns>A string list.</returns>
        internal List<Tuple<string, string>> GetTablesNames()
        {
            List<Tuple<string, string>> tableNames = new List<Tuple<string, string>>();
            string query = "SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";

            try
            {
                SqlDataReader dataReader = queryExecutor.ExecuteQuery(query);

                while (dataReader.Read())
                {
                    tableNames.Add(new Tuple<string,string>(dataReader[0].ToString(), dataReader[1].ToString()));
                }

                dataReader.Close();
                queryExecutor.CloseConnection();

                return tableNames;
            }
            catch (DbException)
            {
                throw;
            }
        }

        /// <summary>
        /// Expands a referenced <paramref name="entityStructure"/> with the attributes.
        /// </summary>
        /// <param name="entityStructure">An EntityStructure reference.</param>
        internal void SetTableAttributes(ref EntityStructure entityStructure, string schema, string name)
        {
            StringBuilder builder = new StringBuilder("SELECT COLUMN_NAME,DATA_TYPE ")
                .Append("FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '")
                .Append(schema)
                .Append("' AND TABLE_NAME = '")
                .Append(name)
                .Append("'");

            try
            {
                SqlDataReader dataReader = queryExecutor.ExecuteQuery(builder.ToString());

                while (dataReader.Read())
                {
                    entityStructure.Attributes.Add(dataReader[0].ToString(), dataReader[1].ToString());
                }

                dataReader.Close();
                queryExecutor.CloseConnection();
            }
            catch (DbException)
            {
                throw;
            }
        }

        /// <summary>
        /// Expands a referenced <paramref name="entityStructure"/> with the primary keys.
        /// </summary>
        /// <param name="entityStructure">An EntityStructure reference.</param>
        internal void SetTablePrimaryKeys(ref EntityStructure entityStructure, string schema, string name)
        {
            StringBuilder builder = new StringBuilder("SELECT COL_NAME(ic.OBJECT_ID,ic.column_id) AS ColumnName ")
                .Append("FROM sys.indexes AS i ")
                .Append("INNER JOIN sys.index_columns AS ic ON i.OBJECT_ID = ic.OBJECT_ID AND i.index_id = ic.index_id ")
                .Append("INNER JOIN sys.objects as obj ON ic.object_id = obj.object_id ")
                .Append("WHERE i.is_primary_key = 1 AND SCHEMA_NAME(schema_id) = '")
                .Append(schema)
                .Append("' AND OBJECT_NAME(ic.OBJECT_ID) = '")
                .Append(name)
                .Append("'");

            try
            {
                SqlDataReader dataReader = queryExecutor.ExecuteQuery(builder.ToString());

                while (dataReader.Read())
                {
                    entityStructure.PrimaryKeys.Add(dataReader[0].ToString());
                }

                dataReader.Close();
                queryExecutor.CloseConnection();
            }
            catch (DbException)
            {
                throw;
            }
        }

        /// <summary>
        /// Expands a referenced <paramref name="entityStructure"/> with the foreign keys.
        /// </summary>
        /// <param name="entityStructure">An EntityStructure reference.</param>
        internal void SetTableForeignKeys(ref EntityStructure entityStructure, string schema, string name)
        {
            StringBuilder builder = new StringBuilder("SELECT ")
                .Append("col1.name AS [column], SCHEMA_NAME(tab2.schema_id) AS referenced_table_schema, ")
                .Append("tab2.name AS referenced_table, col2.name AS referenced_column ")
                .Append("FROM sys.foreign_key_columns fkc ")
                .Append("INNER JOIN sys.tables tab1 ")
                .Append("ON tab1.object_id = fkc.parent_object_id ")
                .Append("INNER JOIN sys.columns col1 ")
                .Append("ON col1.column_id = parent_column_id AND col1.object_id = tab1.object_id ")
                .Append("INNER JOIN sys.tables tab2 ")
                .Append("ON tab2.object_id = fkc.referenced_object_id ")
                .Append("INNER JOIN sys.columns col2 ")
                .Append("ON col2.column_id = referenced_column_id AND col2.object_id = tab2.object_id ")
                .Append("WHERE SCHEMA_NAME(tab1.schema_id) = '")
                .Append(schema)
                .Append("' AND tab1.name = '")
                .Append(name)
                .Append("'");


            try
            {
                SqlDataReader dataReader = queryExecutor.ExecuteQuery(builder.ToString());

                while (dataReader.Read())
                {
                    builder = new StringBuilder(dataReader[1].ToString()).Append(".").Append(dataReader[2].ToString());
                    
                    entityStructure.ForeignKeys.Add(dataReader[0].ToString(),
                        new EntityForeignKey(builder.ToString(), dataReader[3].ToString()));
                }

                dataReader.Close();
                queryExecutor.CloseConnection();
            }
            catch (DbException)
            {
                throw;
            }
        }
    }
}
