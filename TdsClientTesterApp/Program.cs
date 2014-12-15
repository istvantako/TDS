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
        [TdsLoadEntity("TableA", "1")]
        [TdsLoadEntity("TableB", "1")]
        static void TestAnnotation()
        {
            Console.WriteLine("TestAnnotation method.");
        }

        static void Main(string[] args)
        {
            //TdsClient tds = new TdsClient(ConfigurationManager.AppSettings["TdsStoragePath"]);
            //TdsClient tds = new TdsClient("D:\\TDS");
            TdsClient tds = new TdsClient("E:\\TDS");

            //tds.GenerateDatabaseStructure();
            //tds.GetEntityStructures();
            //Console.WriteLine("Struktura generalas sikeres!");

            //var entities = new List<EntityWithKey>();
            //var entity = new EntityWithKey("TableA", new List<string> { "3" });
            //entities.Add(entity);

            var entities = new List<EntityWithKey>();
            var entity = new EntityWithKey("Sales.SalesOrderHeader", new List<string> { "67339" });
            entities.Add(entity);


            //tds.SaveEntities(entities, "D:\\TDS");
            //Console.WriteLine("Mentes sikeres!");

            tds.SaveEntities(entities, "E:\\TDS");
            Console.WriteLine("Mentes sikeres!");


            //tds.LoadEntities(entities, "D:\\TDS");
            //Console.WriteLine("Visszaallitas sikeres!");

            //tds.LoadEntities(entities, "E:\\TDS");
            //Console.WriteLine("Visszaallitas sikeres!");

            //TestAnnotation();

            Console.ReadLine();
        }
    }
}
