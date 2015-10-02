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
        public IRepository SourceRepository { get; set; }

        public DependencyResolver()
        {
        }

        public DependencyResolver(IRepository sourceRepository)
        {
            SourceRepository = sourceRepository;
        }

        public IEnumerable<Entity> GetEntitiesWhereEntityIsDependent(Entity entity, Association association)
        {
            var keyMembers = new List<EntityKey>();

            foreach (var propertyMapping in association.PropertyMappings)
            {
                var keyMember = new EntityKey()
                {
                    Name = propertyMapping.Key,
                    Value = entity.Properties[propertyMapping.Value]
                };
                keyMembers.Add(keyMember);
            }

            return SourceRepository.Read(association.Principal, keyMembers);
        }

        public IEnumerable<Entity> GetEntitiesWhereEntityIsPrincipal(Entity entity, Association association)
        {
            var keyMembers = new List<EntityKey>();

            foreach (var propertyMapping in association.PropertyMappings)
            {
                var keyMember = new EntityKey()
                {
                    Name = propertyMapping.Value,
                    Value = entity.Properties[propertyMapping.Key]
                };
                keyMembers.Add(keyMember);
            }

            return SourceRepository.Read(association.Dependent, keyMembers);
        }
    }
}
