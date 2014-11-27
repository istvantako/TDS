using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;
using TestDataSeeding.Client;
using TestDataSeeding.Model;

namespace TdsConsoleApp
{
    class Program
    {
        /// <summary>
        /// Executes the save entity command, saves the entity given as a list of arguments.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        private static void ExecuteSaveCommand(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Too few or no arguments.");
                Console.WriteLine("Correct calls:");
                Console.WriteLine("    entityName param1 <param2 ..> [other entities separated with semicolons]");
                Console.WriteLine("    -path=targetPath entityName param1 <param2 ..>");
                return;
            }

            

            string path = string.Empty;
            string entityName = string.Empty;
            int startIndex = 1;


            if (args[0].StartsWith("-path="))
            {
                path = args[0].Substring(6);
                entityName = args[1];
                startIndex = 2;
            }
            else
            {
                entityName = args[0];
            }



            TdsClient tdsClient = new TdsClient(ConfigurationManager.AppSettings["TdsStoragePath"]);

            List<EntityWithKey> entities = new List<EntityWithKey>();
            List<string> parameters = new List<string>();
            for (var i = startIndex; i < args.Length; i++)
            {
                

                if (entityName.Equals(string.Empty))
                {
                    entityName = args[i];
                }
                else
                {

                    if (args[i].EndsWith(";"))
                    {
                        args[i] = args[i].Remove(args[i].Length - 1);

                        if (args[i].Length > 0)
                        {
                            parameters.Add(args[i]);
                        }

                        entities.Add(new EntityWithKey(entityName, parameters));

                        Console.WriteLine("Entity name: " + entityName);
                        Console.Write("Parameters: ");
                        foreach (var j in parameters)
                        {
                            Console.Write("'" + j + "'" + ",");
                        }
                        Console.WriteLine();
                        entityName = string.Empty;
                        parameters.Clear();
                    }
                    else
                    {
                        parameters.Add(args[i]);
                    }
                }

                

            }

            entities.Add(new EntityWithKey(entityName, parameters));
            Console.WriteLine("Entity name: " + entityName);
            Console.Write("Parameters: ");
            foreach (var i in parameters)
            {
                Console.Write("'" + i + "'" + ",");
            }
            Console.WriteLine();
            try
            {
                if (path.Equals(string.Empty))
                {
                    tdsClient.SaveEntity(entities);

                }
                else
                {
                    tdsClient.SaveEntity(entities, path);
                }
                Console.WriteLine("The given entities are saved.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        static void Main(string[] args)
        {
            ExecuteSaveCommand(args);
            Console.ReadLine();
        }
    }
}
