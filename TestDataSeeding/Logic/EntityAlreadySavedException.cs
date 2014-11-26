using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.Logic
{
    /// <summary>
    /// Test data seeding business logic exception, the entity has already been saved.
    /// </summary>
    internal class EntityAlreadySavedException : TdsLogicException
    {
        public EntityAlreadySavedException()
        {
        }

        public EntityAlreadySavedException(string message)
            : base(message)
        {
        }

        public EntityAlreadySavedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
