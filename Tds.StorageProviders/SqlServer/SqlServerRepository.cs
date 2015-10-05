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
                    Insert(entity);
                    break;
                case EntityStatus.Modified:
                    Update(entity, keys);
                    break;
            }
        }

        public void SaveChanges()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        using (var command = new SqlCommand("", connection, transaction))
                        {
                            foreach (var query in queries)
                            {
                                command.CommandText = query;
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception)
                {

                }
            }
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
                //try
                //{
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var entity = new Entity();
                                entity.Name = entityName;
                                foreach (var property in entityType.Properties)
                                {
                                    entity.Properties[property.Key] = Converter.ConvertFromString(property.Value, reader[property.Key].ToString());
                                }
                                entities.Add(entity);
                            }
                        }

                    }
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}
            }

            return entities;
        }

        private void Insert(Entity entity)
        {
            //Func<string, string> key = value => String.Concat("@", value);

            var properties = new string[entity.Properties.Count];
            var header = new string[entity.Properties.Count];
            var entityType = MetadataWorkspace.GetEntityType(entity.Name);

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

            queries.Add(query);
            log.Info(query);
        }

        private void Update(Entity entity, ICollection<EntityKey> keys)
        {
            var properties = new string[entity.Properties.Count];
            var keyMembers = new string[keys.Count];
            var entityType = MetadataWorkspace.GetEntityType(entity.Name);

            int index = 0;
            foreach (var property in entity.Properties)
            {
                properties[index] = string.Format("{0} = {1}", property.Key, Converter.ConvertToString(entityType.Properties[property.Key], property.Value));
                index++;
            }

            index = 0;
            foreach (var keyMember in keys)
            {
                keyMembers[index] = string.Format("{0} = {1}", keyMember.Name, Converter.ConvertToString(entityType.Properties[keyMember.Name], keyMember.Value));
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
