using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Engine.Exceptions;
using Tds.Interfaces;
using Tds.Interfaces.Model;
using Tds.Types;

namespace Tds.Engine.Core
{
    class MaintenanceTask
    {
        private IRepository sourceRepository;

        private IRepository targetRepository;

        private IMetadataWorkspace metadataWorkspace;

        private IDependencyResolver dependencyResolver;

        private IEntityTypeFilter filter;

        private ICollection<Entity> visited;

        private EntityComparer entityComparer;

        private ILog log;

        public MaintenanceTask(IMetadataWorkspace metadataWorkspace, IRepository sourceRepository, IRepository targetRepository,
            IDependencyResolver dependencyResolver, IEntityTypeFilter filter)
        {
            this.metadataWorkspace = metadataWorkspace;
            this.sourceRepository = sourceRepository;
            this.targetRepository = targetRepository;
            this.dependencyResolver = dependencyResolver;
            this.filter = filter;
            this.visited = new List<Entity>();
            this.entityComparer = new EntityComparer(metadataWorkspace);

            log4net.Config.XmlConfigurator.Configure();
            this.log = LogManager.GetLogger(typeof(MaintenanceTask));
        }

        public void Save(string entityName, ICollection<EntityKey> keyMembers)
        {
            try
            {
                Entity entity = sourceRepository.Read(entityName, keyMembers).First();
                Save(entity);
                targetRepository.SaveChanges();
            }
            catch (InvalidOperationException exception)
            {
                throw new EntityNotFoundInDatabaseException(entityName, keyMembers, metadataWorkspace.GetEntityType(entityName), exception);
            }
        }

        private class EntityComparer : IEqualityComparer<Entity>
        {
            private IMetadataWorkspace metadataWorkspace;

            public EntityComparer(IMetadataWorkspace metadataWorkspace)
            {
                this.metadataWorkspace = metadataWorkspace;
            }

            public bool Equals(Entity entity, Entity other)
            {
                var entityType = metadataWorkspace.GetEntityType(entity.Name);
                if (entityType == null)
                {
                    throw new EntityTypeNotFoundException(entity.Name);
                }

                if (entity == null || other == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(entity.Name) || string.IsNullOrEmpty(other.Name) || (!entity.Name.Equals(other.Name)))
                {
                    return false;
                }

                if (entity.Properties.Count != other.Properties.Count)
                {
                    return false;
                }

                foreach (var property in entity.Properties)
                {
                    var value = Converter.ConvertToString(entityType.Properties[property.Key], property.Value);
                    var otherValue = Converter.ConvertToString(entityType.Properties[property.Key], other.Properties[property.Key]);

                    if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(otherValue) || (!value.Equals(otherValue)))
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(Entity obj)
            {
                var entityType = metadataWorkspace.GetEntityType(obj.Name);
                if (entityType == null)
                {
                    throw new EntityTypeNotFoundException(obj.Name);
                }

                unchecked
                {
                    int hash = 269;
                    int collectionHash = 0;

                    foreach (var property in obj.Properties.OrderBy(p => p.Key))
                    {
                        collectionHash ^= Converter.ConvertToString(entityType.Properties[property.Key], property.Value).GetHashCode();
                    }

                    hash = (hash * 47) + obj.Name.GetHashCode();
                    hash = (hash * 47) + collectionHash;

                    return hash;
                }
            }
        }

        private void Save(Entity sourceEntity)
        {
            if (visited.Contains(sourceEntity, entityComparer))
            {
                return;
            }
            visited.Add(sourceEntity);

            SaveEntitiesWhereCurrentEntityIsDependent(sourceEntity);

            SaveCurrentEntity(sourceEntity);

            SaveEntitiesWhereCurrentEntityIsPrincipal(sourceEntity);
        }

        private ICollection<EntityKey> GetEntityPrimaryKey(Entity entity)
        {
            var entityType = metadataWorkspace.GetEntityType(entity.Name);
            if (entityType == null)
            {
                throw new EntityTypeNotFoundException(entity.Name);
            }

            var result = new EntityKey[entityType.PrimaryKey.Count];

            int index = 0;
            foreach (var keyMember in entityType.PrimaryKey.OrderBy(member => member.Sequence))
            {
                result[index] = new EntityKey()
                {
                    Name = keyMember.Name,
                    Value = entity.Properties[keyMember.Name]
                };

                index++;
            }

            return result;
        }

        private void SaveCurrentEntity(Entity sourceEntity)
        {
            var primaryKey = GetEntityPrimaryKey(sourceEntity);
            var targetEntities = targetRepository.Read(sourceEntity.Name, primaryKey);

            switch (targetEntities.Count())
            {
                case 0:
                    targetRepository.Write(sourceEntity, primaryKey);
                    break;
                case 1:
                    var targetEntity = targetEntities.First();
                    
                    if (!sourceEntity.Equals(targetEntity))
                    {
                        targetRepository.Write(sourceEntity, primaryKey, EntityStatus.Modified);
                    }
                    break;
                default:
                    break;
            }
        }

        private void SaveEntitiesWhereCurrentEntityIsDependent(Entity sourceEntity)
        {
            foreach (var association in metadataWorkspace.GetAssociationsWhereEntityIsDependent(sourceEntity.Name, filter))
            {
                foreach (var entity in dependencyResolver.GetEntitiesWhereEntityIsDependent(sourceEntity, association))
                {
                    Save(entity);
                }
            }
        }

        private void SaveEntitiesWhereCurrentEntityIsPrincipal(Entity sourceEntity)
        {
            foreach (var association in metadataWorkspace.GetAssociationsWhereEntityIsPrincipal(sourceEntity.Name, filter))
            {
                foreach (var entity in dependencyResolver.GetEntitiesWhereEntityIsPrincipal(sourceEntity, association))
                {
                    Save(entity);
                }
            }
        }
    }
}
