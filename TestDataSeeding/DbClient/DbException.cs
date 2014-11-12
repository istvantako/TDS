using System;

namespace TestDataSeeding.DbClient
{
    public class DbException : Exception
    {
        public DbException()
        {
        }

        public DbException(string message)
            : base(message)
        {
        }

        public DbException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
