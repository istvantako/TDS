using System.Collections.Generic;

using Tds.Interfaces.Model;
using Tds.Interfaces.Metadata;

namespace Tds.Interfaces
{
    public interface IStorageProvider
    {
        IRepository GetRepository(IMetadataProvider metadataProvider);
    }
}
