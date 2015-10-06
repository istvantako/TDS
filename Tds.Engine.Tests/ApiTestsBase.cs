using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces;
using Tds.Interfaces.Model;
using Tds.Types;

namespace Tds.Engine.Tests
{
    [TestClass]
    public abstract class ApiTestsBase
    {
        #region Protected fields --------------------------
        protected static string productionConnectionString;

        protected static string backupConnectionString;

        protected static string drawingsXmlMetadataLocation;

        protected List<string> queries;

        protected ILog log;
        #endregion ----------------------------------------

        public ApiTestsBase()
        {
            queries = new List<string>();

            log4net.Config.XmlConfigurator.Configure();
            this.log = LogManager.GetLogger(typeof(ApiTests));
        }

        #region Helper methods ----------------------------
        protected Entity GetDrawing(int id, string title, int w, int h, int c)
        {
            Entity entity = new Entity();
            entity.Name = "Drawings";
            entity.Properties["Id"] = id;
            entity.Properties["Title"] = title;
            entity.Properties["Width"] = w;
            entity.Properties["Height"] = h;
            entity.Properties["BgColour"] = c;

            return entity;
        }

        protected Entity GetSubDrawing(int mId, int sId, int x, int y, int z)
        {
            Entity entity = new Entity();
            entity.Name = "SubDrawings";
            entity.Properties["MainDrawing"] = mId;
            entity.Properties["SubDrawing"] = sId;
            entity.Properties["X"] = x;
            entity.Properties["Y"] = y;
            entity.Properties["Z"] = z;

            return entity;
        }

        protected Entity GetDrawingsImages(int dId, int iId, int x, int y, int z)
        {
            Entity entity = new Entity();
            entity.Name = "DrawingsImages";
            entity.Properties["DrawingId"] = dId;
            entity.Properties["ImageId"] = iId;
            entity.Properties["X"] = x;
            entity.Properties["Y"] = y;
            entity.Properties["Z"] = z;

            return entity;
        }

        protected Entity GetImage(int id, string url)
        {
            Entity entity = new Entity();
            entity.Name = "Images";
            entity.Properties["Id"] = id;
            entity.Properties["Url"] = url;

            return entity;
        }

        protected Entity GetPixel(int x, int y, int z, int dId, int c)
        {
            Entity entity = new Entity();
            entity.Name = "Pixels";
            entity.Properties["X"] = x;
            entity.Properties["Y"] = y;
            entity.Properties["Z"] = z;
            entity.Properties["DrawingId"] = dId;
            entity.Properties["Colour"] = c;

            return entity;
        }

        protected Entity GetLine(int x, int y, int w, int h, int z, int dId, int c)
        {
            Entity entity = new Entity();
            entity.Name = "Lines";
            entity.Properties["StartX"] = x;
            entity.Properties["StartY"] = y;
            entity.Properties["Width"] = w;
            entity.Properties["Height"] = h;
            entity.Properties["Z"] = z;
            entity.Properties["DrawingId"] = dId;
            entity.Properties["Colour"] = c;

            return entity;
        }

        protected bool CheckEntityExistsInSqlServerDb(string connectionString, IMetadataWorkspace metadataWorkspace, Entity entity)
        {
            var properties = new string[entity.Properties.Count];
            var entityType = metadataWorkspace.GetEntityType(entity.Name);

            int index = 0;
            foreach (var property in entity.Properties)
            {
                properties[index] = string.Format("{0} = {1}", property.Key, Converter.ConvertToString(entityType.Properties[property.Key], property.Value));
                index++;
            }

            var query = string.Format("SELECT COUNT({0}) FROM {1} WHERE {2};",
                "*",
                entity.Name,
                string.Join(" AND ", properties));

            var entityExists = true;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    int count = int.Parse(command.ExecuteScalar().ToString());
                    if (count != 1)
                    {
                        entityExists = false;
                    }
                }
            }

            return entityExists;
        }

        protected void InsertEntityIntoSqlServerDb(string connectionString, IMetadataWorkspace metadataWorkspace, Entity entity)
        {
            var properties = new string[entity.Properties.Count];
            var header = new string[entity.Properties.Count];
            var entityType = metadataWorkspace.GetEntityType(entity.Name);

            int index = 0;
            foreach (var property in entity.Properties)
            {
                properties[index] = Converter.ConvertToString(entityType.Properties[property.Key], property.Value);
                header[index] = property.Key;
                index++;
            }

            var query = string.Format("INSERT INTO {0} ({1}) VALUES ({2});",
                entity.Name,
                string.Join(", ", header),
                string.Join(", ", properties));

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        protected void DeleteEntityFromSqlServerDb(string connectionString, IMetadataWorkspace metadataWorkspace, Entity entity)
        {
            var entityType = metadataWorkspace.GetEntityType(entity.Name);
            var properties = new string[entity.Properties.Count];

            int index = 0;
            foreach (var property in entity.Properties)
            {
                properties[index] = string.Format("{0} = {1}", property.Key, Converter.ConvertToString(entityType.Properties[property.Key], property.Value));
                index++;
            }

            var query = string.Format("DELETE FROM {0} WHERE {1};",
                entity.Name,
                string.Join(" AND ", properties));

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        protected void TruncateSqlServerDb(string connectionString, IEnumerable<string> entities)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var entity in entities)
                {
                    var query = string.Format("TRUNCATE TABLE {0};", entity);
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        #endregion ----------------------------------------
    }
}
