using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces;

namespace Tds.StorageProviders.SqlServer
{
    public class SqlServerStorageProvider : IStorageProvider
    {
        private string connectionString;

        public SqlServerStorageProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IRepository GetRepository(IMetadataProvider metadataProvider)
        {
            return new SqlServerRepository(connectionString, metadataProvider.GetMetadataWorkspace());
        }
    }
}
