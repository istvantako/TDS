using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestDataSeeding.Logic;
using YAXLib;
using TestDataSeeding.Model;
using System.Xml.Linq;

namespace TestDataSeeding.XmlDataAccess
{
    public class XmlHandler : IXmlDataAccess
    {

        public void SaveEntity(Entity entity, EntityStructure entityStructure)
        {
            var serializer = new YAXSerializer(typeof(Entity));
            var xmlName = BuildFileName(entity, entityStructure);
            var wr = new XmlTextWriter(xmlName, null);
            serializer.Serialize(entity, wr); ;
            Console.WriteLine(serializer.Serialize(entity));
            wr.Flush();
        }

        public Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues)
        {
            String xmlFileName = BuildFileName(entityStructure, primaryKeyValues);
            var deserializer = new YAXSerializer(typeof(Entity));
            XmlReader reader = XmlReader.Create(xmlFileName);
            reader.MoveToContent();
            XDocument xdoc = XDocument.Load(reader);

            //reader.Close();
            //var entity =deserializer.DeserializeFromFile(xmlFileName);
            return null;
        }


        public EntityStructures GetEntityStructures()
        {
            throw new NotImplementedException();
        }

        private String BuildFileName(Entity entity, EntityStructure entityStructure)
        {
            String s = entity.Name;
            foreach (var i in entityStructure.PrimaryKeys)
            {
                s += "_" + entity.AttributeValues[i];
            }
            return s + ".xml";
        }

        private string BuildFileName(EntityStructure entityStructure, List<string> primaryKeyValues)
        {
            String s = entityStructure.Name;
            foreach (var i in primaryKeyValues)
            {
                s += "_" + i;
            }
            return s + ".xml";
        }

    }
}