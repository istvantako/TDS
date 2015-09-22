using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tds.Engine.Tests;
using Tds.MetadataProviders.Tests.Xml;
using Tds.Interfaces.Metadata;
using Tds.MetadataProviders.Xml;
using Tds.Types;
using Tds.Interfaces;
using System.IO;

namespace Tds.MetadataProviders.Tests.Data
{
    [TestClass]
    public class XmlMetadataProviderTest : XmlMetadataProviderTestBase
    {
        [TestMethod]
        public void SerializeMetadataWorkspace_Drawings_SubDrawings_Serialized()
        {
            // Arrange
            MetadataWorkspace metadataWorkspace = CreateDrawingsAndSubdrawingsXmlMetadata();
            string xmlMetadataPath = TestSettings.Storage.XmlMetadataGeneratedLocation + @"\Drawings_SubDrawings_Metadata_Generated.xml";
            XmlMetadataProvider metadataProvider = new XmlMetadataProvider(xmlMetadataPath);

            // Act
            metadataProvider.SaveMetadataWorkspace(metadataWorkspace);

            // Assert
            Assert.IsTrue(File.Exists(xmlMetadataPath), "Drawings_SubDrawings XML metadata is not serialized.");
        }

        private MetadataWorkspace CreateDrawingsAndSubdrawingsXmlMetadata()
        {
            EntityType drawingsType = new EntityType();
            drawingsType.Name = "Drawings";
            drawingsType.PrimaryKey.Add(new KeyMember("Id", 0));
            drawingsType.Properties["Id"] = DataType.Integer;
            drawingsType.Properties["Title"] = DataType.String;
            drawingsType.Properties["Width"] = DataType.Integer;
            drawingsType.Properties["Height"] = DataType.Integer;
            drawingsType.Properties["BgColour"] = DataType.Integer;

            EntityType subDrawingsType = new EntityType();
            subDrawingsType.Name = "SubDrawings";
            subDrawingsType.PrimaryKey.Add(new KeyMember("MainDrawing", 0));
            subDrawingsType.PrimaryKey.Add(new KeyMember("SubDrawing", 1));
            subDrawingsType.PrimaryKey.Add(new KeyMember("X", 2));
            subDrawingsType.PrimaryKey.Add(new KeyMember("Y", 3));
            subDrawingsType.PrimaryKey.Add(new KeyMember("Z", 4));
            subDrawingsType.Properties["MainDrawing"] = DataType.Integer;
            subDrawingsType.Properties["SubDrawing"] = DataType.Integer;
            subDrawingsType.Properties["X"] = DataType.Integer;
            subDrawingsType.Properties["Y"] = DataType.Integer;
            subDrawingsType.Properties["Z"] = DataType.Integer;

            Association subdrawingsMainToDrawings = new Association();
            subdrawingsMainToDrawings.Name = "SubDrawings_Drawings_Main";
            subdrawingsMainToDrawings.Principal = "Drawings";
            subdrawingsMainToDrawings.Dependent = "SubDrawings";
            subdrawingsMainToDrawings.PropertyMappings["Id"] = "MainDrawing";

            Association subdrawingsSubsToDrawings = new Association();
            subdrawingsSubsToDrawings.Name = "SubDrawings_Drawings_Sub";
            subdrawingsSubsToDrawings.Principal = "Drawings";
            subdrawingsSubsToDrawings.Dependent = "SubDrawings";
            subdrawingsSubsToDrawings.PropertyMappings["Id"] = "SubDrawing";

            MetadataWorkspace xmlMetadataWorkspace = new MetadataWorkspace();
            xmlMetadataWorkspace.EntityTypes.Add(drawingsType);
            xmlMetadataWorkspace.EntityTypes.Add(subDrawingsType);
            xmlMetadataWorkspace.Associations.Add(subdrawingsMainToDrawings);
            xmlMetadataWorkspace.Associations.Add(subdrawingsSubsToDrawings);

            return xmlMetadataWorkspace;
        }
    }
}
