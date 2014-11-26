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
                Console.WriteLine("    entityName param1 <param2 ..>");
                Console.WriteLine("    -path=targetPath entityName param1 <param2 ..>");
                return;
            }

            List<string> parameters = new List<string>();

            string path = string.Empty;
            string entityName = string.Empty;
            int startIndex = 1;

            if (args[0] != null)
            {
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
            }

            for (var i = startIndex; i < args.Length; i++)
            {
                parameters.Add(args[i]);
            }

            Console.WriteLine("Entity name: " + entityName);
            Console.Write("Parameters: ");
            foreach (var i in parameters)
            {
                Console.Write("'" + i + "'" + ",");
            }
            Console.WriteLine();

            TdsClient tdsClient = new TdsClient(ConfigurationManager.AppSettings["TdsStoragePath"]);

            try
            {
                if (path.Equals(string.Empty))
                {
                    List<EntityWithKey> entities = new List<EntityWithKey>();
                    entities.Add(new EntityWithKey(entityName, parameters));

                    tdsClient.SaveEntity(entities);
                }
                else
                {
                    List<EntityWithKey> entities = new List<EntityWithKey>();
                    entities.Add(new EntityWithKey(entityName, parameters));

                    tdsClient.SaveEntity(entities, path);
                }
                Console.WriteLine("Command executed with success!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Main(string[] args)
        {
            ExecuteSaveCommand(args);
            Console.ReadLine();
        }
    }
}
