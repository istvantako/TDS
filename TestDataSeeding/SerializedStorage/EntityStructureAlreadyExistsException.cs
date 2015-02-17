using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.SerializedStorage
{
    public class EntityStructureAlreadyExistsException:Exception
    {
        public EntityStructureAlreadyExistsException()
        {
        }

        public EntityStructureAlreadyExistsException(String msg)
            : base(msg)
        {
        }

        public EntityStructureAlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
