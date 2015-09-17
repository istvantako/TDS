using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces;

namespace Tds.Engine.Core
{
    class TaskFactory : ITaskFactory
    {
        public IMaintenanceTask CreateBackupTask()
        {
            return new BackupTask();
        }

        public IMaintenanceTask CreateBackupTask(IStorageProvider storageProvider, IMetadataProvider metadataProvider, IDependencyResolver dependencyResolver)
        {
            return new BackupTask(storageProvider, metadataProvider, dependencyResolver);
        }

        public IMaintenanceTask CreateRestoreTask()
        {
            return new RestoreTask();
        }

        public IMaintenanceTask CreateRestoreTask(IStorageProvider storageProvider, IMetadataProvider metadataProvider, IDependencyResolver dependencyResolver)
        {
            return new RestoreTask(storageProvider, metadataProvider, dependencyResolver);
        }
    }
}
