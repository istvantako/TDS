using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;

namespace TestDataSeeding.Client
{
    internal interface IDbStructureManager
    {
        EntityStructures GetDatabaseStructure();
    }
}
