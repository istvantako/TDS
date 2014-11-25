using System;

namespace TestDataSeeding.DbClient
{
    /// <summary>
    /// Database exception class.
    /// </summary>
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
