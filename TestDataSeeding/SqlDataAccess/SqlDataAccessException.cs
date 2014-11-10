using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
