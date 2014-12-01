using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.Logic
{
    /// <summary>
    /// Main Test Data Seeding business logic exception class.
    /// </summary>
    public class TdsLogicException : Exception
    {
        public TdsLogicException()
        {
        }

        public TdsLogicException(string message)
            : base(message)
        {
        }

        public TdsLogicException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
