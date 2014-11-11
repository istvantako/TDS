using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace TestDataSeeding.SqlDataAccess
{
    internal class SqlQueryExecutor
    {
        private SqlConnection connection;
        private string logPath = ConfigurationManager.AppSettings["DatabaseLogPath"] ?? "SQLlog.txt";
        private string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;

        /// <summary>
        /// Opens the database connection.
        /// </summary>
        private void OpenConnection()
        {
            try
            {
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
            catch (SqlException e)
            {
                Log(e.Message);
                CloseConnection();
                throw new SqlDataAccessException(e.Message, e);
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
                SqlCommand sqlCommand = new SqlCommand(command, connection);
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                CloseConnection();
                return rowsAffected;
            }
            catch (SqlException e)
            {
                Log(e.Message);
                CloseConnection();
                throw new SqlDataAccessException(e.Message, e);
            }
        }

        /// <summary>
        /// Appends the line to the SQLlog.txt
        /// </summary>
        /// <param name="line">The string to be appended.</param>
        private void Log(String line)
        {
            StreamWriter file = new StreamWriter(logPath, true);
            file.WriteLine(DateTime.Now + " " + line);

            file.Close();
        }
    }
}
