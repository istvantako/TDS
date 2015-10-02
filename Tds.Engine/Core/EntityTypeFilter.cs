using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces;

namespace Tds.Engine.Core
{
    public class EntityTypeFilter : IEntityTypeFilter
    {
        public IEnumerable<string> EntitiesToSkip { get; set; }

        public EntityTypeFilter()
        {
            EntitiesToSkip = new List<string>();
        }

        public EntityTypeFilter(IEnumerable<string> entitiesToSkip)
        {
            this.EntitiesToSkip = entitiesToSkip;
        }

        public bool IsSkipped(string entityName)
        {
            return EntitiesToSkip.Contains(entityName);
        }
    }
}
