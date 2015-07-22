using System;

namespace Tds.Engine.Exceptions
{
    public class EntityStructureNotFoundException : Exception
    {
        public EntityStructureNotFoundException(string entityName)
            : base(string.Format("No structure found for entity: {0}.", entityName))
        { }
    }
}
