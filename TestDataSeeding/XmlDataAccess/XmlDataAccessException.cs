using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.XmlDataAccess
{
    class XmlDataAccessException:Exception
    {
        public XmlDataAccessException()
        {
        }

        public XmlDataAccessException(string message)
            : base(message)
        {
        }

        public XmlDataAccessException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
