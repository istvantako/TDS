using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.SerializedStorage
{
    /// <summary>
    /// Serialized storage exception class.
    /// </summary>
    class SerializedStorageException : Exception
    {
        public SerializedStorageException()
        {
        }

        public SerializedStorageException(string message)
            : base(message)
        {
        }

        public SerializedStorageException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
