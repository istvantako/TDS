using System;
using System.Data.SqlClient;

namespace TestDataSeeding.SqlDataAccess
{
    public abstract class Sql
    {
        private static bool Connected;
        private static bool ConnectionCreated;

        protected static SqlConnection Connection;
        protected string ConnectionString;

        /// <summary>
        /// Creates a new Database Connection.
        /// </summary>
        private bool CreateConnection()
        {
            if (ConnectionCreated != true)
            {
                try
                {
                    Connection = new SqlConnection(ConnectionString);
                    Connection.Open();
                    ConnectionCreated = true;
                    Connection.Close();
                    Connected = false;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Opens the Connection.
        /// </summary>
        private bool OpenConnection()
        {
            if (Connected != true)
            {
                try
                {
                    CreateConnection();
                    Connection.Open();
                    Connected = true;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Closes the Connection.
        /// </summary>
        private void CloseConnection()
        {
            if (Connected == true)
            {
                Connection.Close();
                Connected = false;
            }
        }

        /// <summary>
        /// Executes a given query, and returns the result in a datareader.
        /// </summary>
        /// <param name="query"> The query to be executed </param>
        /// <param name="errorMessage"> Output error message </param>
        /// <returns>An SqlDataReader with the result of the query</returns>
        protected SqlDataReader ExecuteReader(string query, ref string errorMessage)
        {
            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand(query, Connection);
                SqlDataReader rdr = cmd.ExecuteReader();
                return rdr;
            }
            catch (SqlException e)
            {
                errorMessage = e.Message;
                CloseConnection();
                return null;
            }
        }

        /// <summary>
        /// Executes a given insert/update/delete command, and returns an error message. 
        /// (errormessage is "OK" if no exception occured)
        /// </summary>
        /// <param name="command">The command to be executed</param>
        /// <returns>A string with the error message</returns>
        protected string ExecuteNonQuery(string command)
        {
            string error;
            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand(command, Connection);
                cmd.ExecuteNonQuery();
                error = "OK";
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return error;
        }

        /// <summary>
        /// Closes the data reader given as a parameter, and also closes the connection
        /// </summary>
        /// <param name="datareader">The SqlDataReader to be closed</param>
        protected void CloseDataReader(SqlDataReader datareader)
        {
            if (datareader != null)
                datareader.Close();
            CloseConnection();
        }
    }
}
