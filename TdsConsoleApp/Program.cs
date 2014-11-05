using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;
using TestDataSeeding.Logic;
using TestDataSeeding.SqlDataAccess;
using TestDataSeeding.XmlDataAccess;

namespace TdsConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            /*string connectionString = "Data Source = JARVIS; Initial Catalog = TestBase; Integrated Security = SSPI";

            EntityStructure et = new EntityStructure("Berlesek");
            et.Attributes.Add("BerlesID", "int");
            et.Attributes.Add("NyaraloID", "int");
            et.Attributes.Add("BerloNev", "int");
            et.PrimaryKeys.Add("BerlesID");
            et.PrimaryKeys.Add("BerloNev");

            et.ForeignKeys.Add("NyaraloID", new EntityForeignKey("Nyaralok", "NyaraloID"));

            List<String> keys = new List<String> { "4", "Fulop Emese" };

            ISqlDataAccess acc = new SqlDataAccess();
            acc.SetConnectionString(connectionString);
            Entity tty = acc.GetEntity(et, keys);
            Console.WriteLine(tty);
            acc.SaveEntity(tty, et);*/

            /*Entity e1 = new Entity("Gyumi");
            e1.AttributeValues.Add("Gyumolcs", "Alma");
            Console.WriteLine(e1);

            Entity e2 = new Entity("Gyumi");
            e2.AttributeValues.Add("Gyumolcs", "Alma");
            Console.WriteLine(e2);

            Entity e3 = new Entity("Gyumi");
            e3.AttributeValues.Add("Gyumolcs", "Korte");
            Console.WriteLine(e3);

            Entity e4 = new Entity("Busz");
            e4.AttributeValues.Add("Marka", "Saviem");
            Console.WriteLine(e4);

            Console.WriteLine(e1.Equals(e2));
            Console.WriteLine(e1.Equals(e3));
            Console.WriteLine(e1.Equals(e4));
            Console.WriteLine();*/

            Entity e5 = new Entity("Gyumi");
            e5.AttributeValues.Add("Gyumolcs", "Alma");
            e5.AttributeValues.Add("Szarmazas", "Romania");
            e5.AttributeValues.Add("Mennyiseg", "100");
            Console.WriteLine(e5);

            EntityStructure et = new EntityStructure("Gyumi");
            et.Attributes.Add("Gyumolcs", "string");
            et.Attributes.Add("Szarmazas", "string");
            et.Attributes.Add("Mennyiseg", "int");
            et.PrimaryKeys.Add("Gyumolcs");
            et.PrimaryKeys.Add("Szarmazas");
            et.ForeignKeys.Add("Gyumolcs", new EntityForeignKey("Rendelesek", "GyumiId"));

            EntityStructures ets = new EntityStructures();
            ets.Add(et);
            foreach (var et1 in ets)
            {
                Console.WriteLine(et1);
            }

            IXmlDataAccess xmlDataAccess = new XmlHandler();
            Console.WriteLine("Saving entity ...");
            xmlDataAccess.SaveEntity(e5, et, "E:");
            Console.WriteLine("Saved.");
            Entity e = xmlDataAccess.GetEntity(et, new List<string>() { "Alma", "Romania" }, "E:");
            Console.WriteLine("Deserialized:");
            Console.WriteLine(e);
            Console.ReadLine();
        }
    }
}
