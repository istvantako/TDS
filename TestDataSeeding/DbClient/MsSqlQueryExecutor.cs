using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace TestDataSeeding.DbClient
{
    internal class MsSqlQueryExecutor
    {
        private SqlConnection connection;

        /// <summary>
        /// Opens the database connection.
        /// </summary>
        private void OpenConnection()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Closes the database Connection.
        /// </summary>
        internal void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Executes a given sql Select query.
        /// </summary>
        /// <param name="query"> The query to be executed </param>
        /// <returns>An SqlDataReader with the result of the query</returns>
        internal SqlDataReader ExecuteQuery(string query)
        {
            Log(query);
            try
            {
                OpenConnection();
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                return dataReader;
            }
            catch (Exception e)
            {
                CloseConnection();
                if (e is SqlException)
                {
                    Log(e.Message);
                    throw new DbException(e.Message, e);
                }
                else
                {
                    throw new DbException("Corrupt App config. Invalid connection string.", e);
                }
            }
        }

        /// <summary>
        /// Executes a given insert/update/delete command.
        /// </summary>
        /// <param name="command">The command to be executed</param>
        /// <returns>Number of rows affected</returns>
        internal int ExecuteNonQuery(string command)
        {
            Log(command);
            try
            {
                OpenConnection();
                SqlCommand sqlCommand = new SqlCommand(command, connection);
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                CloseConnection();
                return rowsAffected;
            }
            catch (Exception e)
            {
                CloseConnection();
                if (e is SqlException)
                {
                    Log(e.Message);
                    throw new DbException(e.Message, e);
                }
                else
                {
                    throw new DbException("Corrupt App config. Invalid connection string.", e);
                }
            }
        }

        /// <summary>
        /// Appends the line to the SQLlog.txt
        /// </summary>
        /// <param name="line">The string to be appended.</param>
        private void Log(String line)
        {
            try
            {
                StreamWriter file = new StreamWriter(ConfigurationManager.AppSettings["DatabaseLogPath"], true);
                file.WriteLine(DateTime.Now + " " + line);
                file.Close();
            }
            catch (Exception e)
            {
                throw new DbException("Corrupt App config. Invalid database logfile path.", e);
            }
        }
    }
}
