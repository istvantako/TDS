using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Tds.Common;
using Tds.Interfaces;
using Tds.Interfaces.Metadata;

namespace Tds.MetadataProviders.Xml
{
    public class XmlMetadataProvider : IMetadataProvider
    {
        public string XmlFilePath { get; set; }

        public XmlMetadataProvider()
        {
            XmlFilePath = string.Empty;
        }

        public XmlMetadataProvider(string xmlFilePath)
        {
            XmlFilePath = xmlFilePath;
        }

        public IMetadataWorkspace GetMetadataWorkspace()
        {
            return XmlSerializer.Deserialize<MetadataWorkspace>(XmlFilePath);
        }

        public void SaveMetadataWorkspace(MetadataWorkspace metadataWorkspace)
        {
            XmlSerializer.Serialize<MetadataWorkspace>(metadataWorkspace, XmlFilePath);
        }
    }
}
