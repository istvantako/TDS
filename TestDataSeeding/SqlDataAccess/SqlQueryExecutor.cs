using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace TestDataSeeding.SqlDataAccess
{
    internal class SqlQueryExecutor
    {
        private SqlConnection connection;
        private string logPath;
    
        /// <summary>
        /// Opens the database connection.
        /// </summary>
        /// <param name="connectionString">An SQL connection string.</param>
        /// <returns>Returns wether the connection could be opened or not.</returns>
        internal bool OpenConnection()
        {
            logPath = ConfigurationManager.AppSettings["DatabaseLogPath"] ?? "SQLlog.txt";

            CloseConnection();
            string connectionString = GetConnectionStringByName("DatabaseConnection");

            if (connectionString != null)
            {
                try
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
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
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                return dataReader;
            }
            catch (SqlException e)
            {
                Log(e.Message);
                return null;
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
                return rowsAffected;
            }
            catch (Exception e)
            {
                Log(e.Message);
                return -1;
            }
        }

        static string GetConnectionStringByName(string name)
        {
            string returnValue = null;

            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[name];

            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }

        /// <summary>
        /// Appends the line to the SQLlog.txt
        /// </summary>
        /// <param name="line">The string to be appended.</param>
        private void Log(String line)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(logPath, true);
            file.WriteLine(DateTime.Now + " " + line);

            file.Close();
        }
    }
}
