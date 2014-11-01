using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;

namespace TdsConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
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

            Console.ReadLine();
        }
    }
}
