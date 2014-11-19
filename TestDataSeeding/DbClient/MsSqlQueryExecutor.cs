using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

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
        /// Executes all or none of the INSERT/UPDATE statements contained in <paramref name="statements"/>
        /// </summary>
        /// <param name="statements">The INSERT/UPDATE statements to be executed</param>
        internal void ExecuteTransaction(List<string> statements)
        {
            try
            {
                OpenConnection();

                Log("Begin transaction.");
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("TDSTransaction");

                // Must assign both transaction object and connection 
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    int rowsAffected;
                    foreach (var statement in statements)
                    {
                        //execute statement
                        command.CommandText = statement;
                        Log(statement);
                        rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected != 1)
                        {
                            throw (new DbException("Transaction failure, see log file."));
                        }
                    }

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    Log("Commit transaction.");
                }
                catch
                {
                    // Attempt to roll back the transaction. 
                    try
                    {
                        Log("Rollback transaction.");
                        transaction.Rollback();
                        CloseConnection();
                    }
                    catch
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as 
                        // a closed connection.
                        throw;
                    }
                    throw;
                }
            }
            //if the connection could not be opened or transaction fails
            catch (Exception e)
            {
                CloseConnection();
                if ((e is SqlException) || (e is DbException))
                {
                    Log(e.Message);
                    throw new DbException(e.Message, e);
                }
                //if not SqlException or DbException it has to connectionString format, or null Exception
                else
                {
                    throw new DbException("Corrupt App config. Invalid connection string.", e);
                }
            }
        }


        /// <summary>
        /// Appends the <paramref name="line"/> to the file specified in App.config as "DatabaseLogPath"
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
