using System;
using System.Data;
using System.Data.SqlClient;

namespace TestDataSeeding.SqlDataAccess
{
    public abstract class Sql
    {
        private SqlConnection connection;

        /// <summary>
        /// Opens the database connection.
        /// </summary>
        /// <param name="connectionString">An SQL connection string.</param>
        /// <returns>Returns wether the connection could be opened or not.</returns>
        protected bool OpenConnectionWithString(string connectionString)
        {
            CloseConnection();
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

        /// <summary>
        /// Opens the database connection.
        /// </summary>
        /// <param name="sqlConnection">An sqlConnection object.</param>
        /// <returns>Returns wether the connection could be opened or not.</returns>
        protected bool OpenConnection(SqlConnection sqlConnection)
        {
            CloseConnection();
            try
            {
                connection = sqlConnection;
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Closes the database Connection.
        /// </summary>
        protected void CloseConnection()
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
        protected SqlDataReader ExecuteQuery(string query)
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
        protected int ExecuteNonQuery(string command)
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

        /// <summary>
        /// Appends the line to the SQLlog.txt
        /// </summary>
        /// <param name="line">The string to be appended.</param>
        private void Log(String line)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("SQLlog.txt", true);
            file.WriteLine(DateTime.Now + " " + line);

            file.Close();
        }
    }
}
