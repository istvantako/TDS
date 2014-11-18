using System;
using System.Collections.Generic;
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
            XmlStorageClient xml = new XmlStorageClient();

            //EntityStructure esA = new EntityStructure();
            //esA.Name = "TableA";
            //esA.Attributes.Add("TableA_ID", "int");
            //esA.Attributes.Add("TableB_ID", "int");
            //esA.Attributes.Add("TableC_ID", "int");
            //esA.Attributes.Add("name", "string");
            //esA.PrimaryKeys.Add("TableA_ID");
            //esA.ForeignKeys.Add("TableB_ID", new EntityForeignKey("TableB", "TableB_ID"));
            //esA.ForeignKeys.Add("TableC_ID", new EntityForeignKey("TableC", "TableC_ID"));

            //EntityStructure esB = new EntityStructure();
            //esB.Name = "TableB";
            //esB.Attributes.Add("TableB_ID", "int");
            //esB.Attributes.Add("name", "string");
            //esB.PrimaryKeys.Add("TableB_ID");

            //EntityStructure esC = new EntityStructure();
            //esC.Name = "TableC";
            //esC.Attributes.Add("TableC_ID", "int");
            //esC.Attributes.Add("name", "string");
            //esC.PrimaryKeys.Add("TableC_ID");

            //EntityStructures escollection = new EntityStructures();
            //escollection.Add(esA);
            //escollection.Add(esB);
            //escollection.Add(esC);


            //xml.SaveEntityStructures(escollection, "D:\\TDS");

            var a = xml.GetEntityStructures("D:\\TDS");
            foreach (var b in a.structures)
            {
                Console.WriteLine(b);
            }
            //// Ez eddig OK!
            

            //TdsClient tds = new TdsClient();
            //tds.SaveEntity("TableA", new List<string> { "1" });
            //tds.SaveEntity("TableA", new List<string> { "2" });
            //tds.SaveEntity("TableA", new List<string> { "3" });
            //Console.WriteLine("Mentes sikeres!");
            //// Meglepo, de mukodik!
            

            /**
             * Es itt kezzel belepiszkalunk az adatbazisba :)
             */

            //TdsClient tds = new TdsClient();
            //tds.LoadEntity("TableA", new List<string> { "1" });
            //tds.LoadEntity("TableA", new List<string> { "2" });
            //Console.WriteLine("Visszaallitas sikeres!");
            //// Ez vegkepp meglepo, de mukodik, es elsore!


            Console.ReadLine();
        }
    }
}
