using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Tds.Interfaces;
using Tds.Interfaces.Model;
using Tds.Types;

namespace Tds.StorageProviders.SqlServer
{
    public class SqlServerRepository : IRepository
    {
        private List<string> queries;

        private string connectionString;

        private ILog log;

        public IMetadataWorkspace MetadataWorkspace { get; set; }

        public SqlServerRepository(string connectionString, IMetadataWorkspace metadataWorkspace)
        {
            this.queries = new List<string>();

            this.connectionString = connectionString;
            this.MetadataWorkspace = metadataWorkspace;

            log4net.Config.XmlConfigurator.Configure();
            this.log = LogManager.GetLogger(typeof(SqlServerRepository));
        }

        public IEnumerable<Entity> Read(string entityName, ICollection<EntityKey> keys)
        {
            return Get(entityName, keys);
        }

        public void Write(Entity entity, ICollection<EntityKey> keys, EntityStatus status = EntityStatus.Added)
        {
            switch (status)
            {
                case EntityStatus.Added:
                    Insert(entity, keys);
                    break;
                case EntityStatus.Modified:
                    Update(entity, keys);
                    break;
            }
        }

        public void SaveChanges()
        {
            
        }

        private IEnumerable<Entity> Get(string entityName, ICollection<EntityKey> keys)
        {
            var keyMembers = new string[keys.Count];

            int index = 0;
            foreach (var keyMember in keys)
            {
                keyMembers[index] = string.Format("{0} = {1}", keyMember.Name, keyMember.Value);
                index++;
            }

            var query = string.Format("SELECT {0} FROM {1} WHERE {2};",
                "*",
                entityName,
                string.Join(" AND ", keyMembers));
            log.Info(query);

            var entities = new List<Entity>();
            var entityType = MetadataWorkspace.GetEntityType(entityName);

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var entity = new Entity();
                            foreach (var property in entityType.Properties)
                            {
                                entity.Properties[property.Key] = Converter.ConvertFromString(property.Value, reader[property.Key].ToString());
                            }
                            entities.Add(entity);
                        }

                    }
                }
                catch (Exception)
                {

                }
            }

            return entities;
        }

        private void Insert(Entity entity, ICollection<EntityKey> keys)
        {
            //Func<string, string> key = value => String.Concat("@", value);

            var fields = from pair in entity.Properties
                         orderby pair.Key ascending
                         select pair.Key;

            var values = from pair in entity.Properties
                         orderby pair.Key ascending
                         select pair.Value;

            var query = string.Format("INSERT INTO {0} ({1}) VALUES ({1});",
                entity.Name,
                string.Join(", ", fields.ToArray()),
                string.Join(", ", values.ToArray()));

            queries.Add(query);
            log.Info(query);
        }

        private void Update(Entity entity, ICollection<EntityKey> keys)
        {
            var properties = new string[entity.Properties.Count];
            var keyMembers = new string[keys.Count];

            int index = 0;
            foreach (var property in entity.Properties)
            {
                properties[index] = string.Format("{0} = {1}", property.Key, property.Value);
                index++;
            }

            index = 0;
            foreach (var keyMember in keys)
            {
                keyMembers[index] = string.Format("{0} = {1}", keyMember.Name, keyMember.Value);
                index++;
            }

            var query = string.Format("UPDATE {0} SET {1} WHERE {2};",
                entity.Name,
                string.Join(", ", properties),
                string.Join(" AND ", keyMembers));

            queries.Add(query);
            log.Info(query);
        }
    }
}
