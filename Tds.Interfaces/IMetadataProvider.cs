using System.Collections.Generic;
using Tds.Interfaces.Metadata;

namespace Tds.Interfaces
{
    public interface IMetadataProvider
    {
        IMetadataWorkspace GetMetadataWorkspace();
    }
}
