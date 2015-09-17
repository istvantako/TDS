using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tds.MetadataProviders.Test.Xml
{
    [TestClass]
    public abstract class XmlMetadataProviderTestBase
    {
        #region Test initialization -----------------------
        [TestInitialize]
        public void InitializeTest()
        {
            // We need to make sure the directories used for testing.
            if (!Directory.Exists(TestSettings.Storage.XmlMetadataManualLocation))
            {
                Directory.CreateDirectory(TestSettings.Storage.XmlMetadataManualLocation);
            }

            if (!Directory.Exists(TestSettings.Storage.XmlMetadataGeneratedLocation))
            {
                Directory.CreateDirectory(TestSettings.Storage.XmlMetadataGeneratedLocation);
            }

            // Delete all the generated XML metadata.
            foreach (var filePath in Directory.GetFiles(TestSettings.Storage.XmlMetadataGeneratedLocation))
            {
                File.Delete(filePath);
            }
        }
        #endregion ----------------------------------------
    }
}
