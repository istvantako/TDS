using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces;

namespace Tds.Engine.Core
{
    /// <summary>
    /// 
    /// </summary>
    interface IMaintenanceTask
    {
        IStorageProvider StorageProvider { get; set; }

        IMetadataProvider MetadataProvider { get; set; }

        IDependencyResolver DependencyResolver { get; set; }

        void Execute();
    }
}
