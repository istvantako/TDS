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
    public class XmlMetadataSerializer : IMetadataProvider
    {
        public string XmlFilePath { get; set; }

        public XmlMetadataSerializer()
        {
            XmlFilePath = string.Empty;
        }

        public XmlMetadataSerializer(string xmlFilePath)
        {
            XmlFilePath = xmlFilePath;
        }

        public IMetadataWorkspace GetMetadataWorkspace()
        {
            return XmlSerializer.Deserialize<MetadataWorkspace>(XmlFilePath);
        }
    }
}
