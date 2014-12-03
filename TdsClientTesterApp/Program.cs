using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Client;
using TestDataSeeding.Model;
using TestDataSeeding.SerializedStorage;

namespace TdsClientTesterApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TdsClient tds = new TdsClient(ConfigurationManager.AppSettings["TdsStoragePath"]);

            //tds.GenerateDatabaseStructure();

            var entities = new List<EntityWithKey>();
            var entity = new EntityWithKey("TableA", new List<string> { "1" });
            entities.Add(entity);
            //entity = new EntityWithKey("TableA", new List<string> { "2" });
            //entities.Add(entity);

            //tds.SaveEntities(entities, "D:\\TDS");
            //Console.WriteLine("Mentes sikeres!");


            //tds.LoadEntities(entities);
            //Console.WriteLine("Visszaallitas sikeres!");


            Console.ReadLine();
        }
    }
}
