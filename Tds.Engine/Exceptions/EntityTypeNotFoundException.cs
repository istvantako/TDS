using System;

namespace Tds.Engine.Exceptions
{
    public class EntityTypeNotFoundException : Exception
    {
        public EntityTypeNotFoundException(string entityName)
            : base(string.Format("No structure found for entity: {0}.", entityName))
        { }
    }
}
