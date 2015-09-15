using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces;
using Tds.Interfaces.Structure;
using YAXLib;

namespace Tds.StructureProviders.Xml
{
    [YAXSerializeAs("EntityStructures")]
    public class XmlStructureProvider : IStructureProvider
    {
        [YAXCollection(YAXCollectionSerializationTypes.Recursive), YAXSerializeAs("Entities")]
        public override IEnumerable<IEntityStructure> EntityStructures { get; set; }

        public XmlStructureProvider()
        {
            EntityStructures = new List<IEntityStructure>();
        }

        public IEntityStructure GetEntityStructure(string entityName)
        {
            if (entityName == null)
            {
                throw new ArgumentNullException("The entity name object is null!");
            }

            return EntityStructures.Where(structure => structure.Type.Equals(entityName)).First();
        }
    }
}
