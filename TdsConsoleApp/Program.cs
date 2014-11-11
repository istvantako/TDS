using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;
using TestDataSeeding.Logic;
using TestDataSeeding.SqlDataAccess;
using System.Diagnostics;
using TestDataSeeding.XmlDataAccess;
using System.Configuration;

namespace TdsConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            run(args);
            //===============================================================
            //Beni
            /*EntityStructure structure = new EntityStructure("Termekek");
            structure.Attributes.Add("TermekID", "int");
            structure.PrimaryKeys.Add("TermekID");

            List<String> keys = new List<String> { "2" };

            SqlDataAccess access = new SqlDataAccess();
            Entity entity;
            try
            {
                entity = access.GetEntity(structure, keys);
                Console.WriteLine(entity);
            }
            catch(SqlDataAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.Read();
            */
            //Beni
            //===================================================================================
            //Lajos
            /*
            Entity e1 = new Entity("Gyumi");
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
            Console.WriteLine();

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

            XmlHandler xmlDataAccess = new XmlHandler();
            Console.WriteLine("Saving entity ...");
            xmlDataAccess.SaveEntity(e5, et, "D:");
            Console.WriteLine("Saved.");
            Entity e = xmlDataAccess.GetEntity(et, new List<string>() { "Alma", "Romania" }, "D:");
            Console.WriteLine("Deserialized:");
            Console.WriteLine(e);
            
            xmlDataAccess.SaveStructure(et, "D://structures", "structure2.xml");
            */
            //Mark
            //==============================================================================
            
            //IXmlDataAccess xmlDataAccess = new XmlHandler();
            /*var structures = xmlDataAccess.GetEntityStructures("D:\\structures");
            var enumerator = structures.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current);
            }
            */
            Console.ReadLine();
            
        }

        private static void run(string [] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments");
                return;
            }
            List<string> parameters = new List<string>();
            string entityName = args[0];
            for (var i = 1; i < args.Length; i++)
            {
                parameters.Add(args[i]);
            }

            Console.WriteLine("Entity name: " +entityName);
            Console.WriteLine("Parameters:");
            foreach (var i in parameters)
            {
                Console.Write("'" + i + "'" + ","); 
            }

            try
            {
                //TODO
                //hivja meg a logika save entity metodusat, vmi.SaveEntity(entityName, parameters)-el
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
            }


        }
    }
}
