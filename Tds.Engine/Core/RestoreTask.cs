using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces;

namespace Tds.Engine.Core
{
    class RestoreTask : IMaintenanceTask
    {
        public IStorageProvider StorageProvider { get; set; }

        public IMetadataProvider MetadataProvider { get; set; }

        public IDependencyResolver DependencyResolver { get; set; }

        public RestoreTask()
        {
        }

        public RestoreTask(IStorageProvider storageProvider, IMetadataProvider metadataProvider, IDependencyResolver dependencyResolver)
        {
            StorageProvider = storageProvider;
            MetadataProvider = metadataProvider;
            DependencyResolver = dependencyResolver;
        }

        public void Execute()
        {
            
        }
    }
}
