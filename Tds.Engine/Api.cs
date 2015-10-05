using System.Collections.Generic;
using System.Linq;
using Tds.Engine.Exceptions;
using Tds.Interfaces;
using Tds.Interfaces.Model;
using Tds.Interfaces.Metadata;
using Tds.Types;
using Tds.Engine.Core;
using System;

namespace Tds.Engine
{
    public class Api
    {
        #region Private fields
        private IMetadataWorkspace metadataWorkspace;
        private IRepository productionRepository;
        private IRepository backupRepository;
        #endregion

        #region Constructurs
        public Api(IMetadataProvider metadataProvider, IStorageProvider productionStorageProvider, IStorageProvider backupStorageProvider)
        {
            this.metadataWorkspace = metadataProvider.GetMetadataWorkspace();

            this.productionRepository = productionStorageProvider.GetRepository(metadataProvider);
            this.backupRepository = backupStorageProvider.GetRepository(metadataProvider);
        }
        #endregion

        #region Public methods
        public void Backup(string entityName, IEnumerable<string> keys, IEnumerable<string> entitiesToSkip = null)
        {
            EntityType entityType = GetEntityType(entityName);
            IEntityTypeFilter filter = SetUpFilter(entitiesToSkip);
            IDependencyResolver dependencyResolver = new DependencyResolver(productionRepository);

            var backupTask = new MaintenanceTask(metadataWorkspace, productionRepository, backupRepository, dependencyResolver, filter);
            var entityKeyMembers = GetEntityKeys(entityType, keys.ToArray());

            backupTask.Save(entityName, entityKeyMembers);
        }

        public void Restore(string entityName, IEnumerable<string> keys, IEnumerable<string> entitiesToSkip = null)
        {
            EntityType entityType = GetEntityType(entityName);
            IEntityTypeFilter filter = SetUpFilter(entitiesToSkip);
            IDependencyResolver dependencyResolver = new DependencyResolver(backupRepository);

            var restoreTask = new MaintenanceTask(metadataWorkspace, backupRepository, productionRepository, dependencyResolver, filter);
            var entityKeyMembers = GetEntityKeys(entityType, keys.ToArray());

            restoreTask.Save(entityName, entityKeyMembers);
        }
        #endregion

        #region Private methods
        private EntityType GetEntityType(string entityName)
        {
            EntityType entityType = metadataWorkspace.GetEntityType(entityName);
            if (entityType == null)
            {
                throw new EntityTypeNotFoundException(entityName);
            }

            return entityType;
        }

        private IEntityTypeFilter SetUpFilter(IEnumerable<string> entitiesToSkip)
        {
            EntityTypeFilter filter = new EntityTypeFilter();
            if (entitiesToSkip != null)
            {
                foreach (var entityToSkip in entitiesToSkip)
                {
                    if (metadataWorkspace.GetEntityType(entityToSkip) == null)
                    {
                        throw new EntityTypeNotFoundException(entityToSkip + " [in filter]");
                    }
                }

                filter.EntitiesToSkip = entitiesToSkip;
            }

            return filter;
        }

        private ICollection<EntityKey> GetEntityKeys(EntityType entityType, string[] keys)
        {
            var keyMembers = entityType.PrimaryKey;
            var properties = entityType.Properties;

            var result = new EntityKey[keyMembers.Count];
            
            var index = 0;
            foreach (var item in keyMembers.OrderBy(x => x.Sequence))
	        {
                try
                {
                    result[index] = new EntityKey()
                    {
                        Name = item.Name,
                        Value = Converter.ConvertFromString(properties[item.Name], keys[index])
                    };
                }
                catch (IndexOutOfRangeException)
                {
                    throw new MissingKeyMemberException();
                }

                index++;
	        }

            return result;
        }
        #endregion
    }
}
