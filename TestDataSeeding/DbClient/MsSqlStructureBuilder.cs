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
        public List<string> GetTablesNames()
        {
            List<string> tableNames = new List<string>();
            string query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";

            try
            {
                SqlDataReader dataReader = queryExecutor.ExecuteQuery(query);

                while (dataReader.Read())
                {
                    tableNames.Add(dataReader[0].ToString());
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
        public void SetTableAttributes(ref EntityStructure entityStructure)
        {
            StringBuilder builder = new StringBuilder("SELECT COLUMN_NAME,DATA_TYPE ");
            builder.Append("FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '");
            builder.Append(entityStructure.Name);
            builder.Append("'");

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
        public void SetTablePrimaryKeys(ref EntityStructure entityStructure)
        {
            StringBuilder builder = new StringBuilder("SELECT COLUMN_NAME ");
            builder.Append("FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE ");
            builder.Append("WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 AND table_name = '");
            builder.Append(entityStructure.Name);
            builder.Append("'");

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
        public void SetTableForeignKeys(ref EntityStructure entityStructure)
        {
            StringBuilder builder = new StringBuilder("SELECT ");
            builder.Append("col1.name AS [column],tab2.name AS referenced_table,col2.name AS referenced_column ");
            builder.Append("FROM sys.foreign_key_columns fkc ");
            builder.Append("INNER JOIN sys.tables tab1 ");
            builder.Append("ON tab1.object_id = fkc.parent_object_id ");
            builder.Append("INNER JOIN sys.columns col1 ");
            builder.Append("ON col1.column_id = parent_column_id AND col1.object_id = tab1.object_id ");
            builder.Append("INNER JOIN sys.tables tab2 ");
            builder.Append("ON tab2.object_id = fkc.referenced_object_id ");
            builder.Append("INNER JOIN sys.columns col2 ");
            builder.Append("ON col2.column_id = referenced_column_id AND col2.object_id = tab2.object_id ");
            builder.Append("WHERE tab1.name = '");
            builder.Append(entityStructure.Name);
            builder.Append("'");

            try
            {
                SqlDataReader dataReader = queryExecutor.ExecuteQuery(builder.ToString());

                while (dataReader.Read())
                {
                    entityStructure.ForeignKeys.Add(dataReader[0].ToString(), 
                        new EntityForeignKey(dataReader[1].ToString(),dataReader[2].ToString()));
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
