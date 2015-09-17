using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces;

namespace Tds.Engine.Core
{
    interface ITaskFactory
    {
        IMaintenanceTask CreateBackupTask();

        IMaintenanceTask CreateBackupTask(IStorageProvider storageProvider, IMetadataProvider metadataProvider, IDependencyResolver dependencyResolver);

        IMaintenanceTask CreateRestoreTask();

        IMaintenanceTask CreateRestoreTask(IStorageProvider storageProvider, IMetadataProvider metadataProvider, IDependencyResolver dependencyResolver);
    }
}
