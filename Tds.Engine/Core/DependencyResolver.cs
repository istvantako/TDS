using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces;
using Tds.Interfaces.Metadata;
using Tds.Interfaces.Model;

namespace Tds.Engine.Core
{
    class DependencyResolver : IDependencyResolver
    {
        public IMetadataWorkspace MetadataWorkspace { get; set; }

        public IRepository SourceRepository { get; set; }

        public IEnumerable<Entity> GetEntitiesWhereEntityIsDependent(Entity entity, Association association)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entity> GetEntitiesWhereEntityIsPrincipal(Entity entity, Association association)
        {
            throw new NotImplementedException();
        }
    }
}
