using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Engine.Exceptions;
using Tds.Interfaces;
using Tds.Interfaces.Model;

namespace Tds.Engine.Core
{
    class MaintenanceTask
    {
        private IRepository sourceRepository { get; set; }

        private IRepository targetRepository { get; set; }

        private IMetadataWorkspace metadataWorkspace { get; set; }

        private IDependencyResolver dependencyResolver { get; set; }

        public MaintenanceTask(IMetadataWorkspace metadataWorkspace, IRepository sourceRepository, IRepository targetRepository,
            IDependencyResolver dependencyResolver)
        {
            this.metadataWorkspace = metadataWorkspace;
            this.sourceRepository = sourceRepository;
            this.targetRepository = targetRepository;
            this.dependencyResolver = dependencyResolver;
        }

        public void Save(string entityName, ICollection<EntityKey> keyMembers)
        {
            Entity entity = sourceRepository.Read(entityName, keyMembers).First();

            if (entity != null)
            {
                Save(entity);
            }
            else
            {
                throw new EntityNotFoundInDatabaseException(entity.Name, keyMembers, metadataWorkspace.GetEntityType(entity.Name));
            }

            targetRepository.SaveChanges();
        }

        private void Save(Entity sourceEntity)
        {
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
            var targetEntity = targetRepository.Read(sourceEntity.Name, primaryKey);

            if (targetEntity != null)
            {
                if (!sourceEntity.Equals(targetEntity))
                {
                    targetRepository.Write(sourceEntity, primaryKey, EntityStatus.Modified);
                }
            }
            else
            {
                targetRepository.Write(sourceEntity, primaryKey);
            }
        }

        private void SaveEntitiesWhereCurrentEntityIsDependent(Entity sourceEntity)
        {
            foreach (var association in metadataWorkspace.GetAssociationsWhereEntityIsDependent(sourceEntity.Name))
            {
                foreach (var entity in dependencyResolver.GetEntitiesWhereEntityIsDependent(sourceEntity, association))
                {
                    Save(entity);
                }
            }
        }

        private void SaveEntitiesWhereCurrentEntityIsPrincipal(Entity sourceEntity)
        {
            foreach (var association in metadataWorkspace.GetAssociationsWhereEntityIsPrincipal(sourceEntity.Name))
            {
                foreach (var entity in dependencyResolver.GetEntitiesWhereEntityIsPrincipal(sourceEntity, association))
                {
                    Save(entity);
                }
            }
        }
    }
}
