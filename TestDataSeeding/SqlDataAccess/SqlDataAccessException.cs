using System;

namespace TestDataSeeding.SqlDataAccess
{
    public class SqlDataAccessException : Exception
    {
        public SqlDataAccessException()
        {
        }

        public SqlDataAccessException(string message)
            : base(message)
        {
        }

        public SqlDataAccessException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
