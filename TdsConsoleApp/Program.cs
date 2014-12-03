using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;
using TestDataSeeding.Client;
using TestDataSeeding.Model;
using TestDataSeeding.Logic;

namespace TdsConsoleApp
{
    class Program
    {
        private static EntityWithKey entity;
        private static string path = string.Empty;

        /// <summary>
        /// Validates and sets the entity and path members based on the arguments.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        /// <returns>Return true if the arguments were valid.</returns>
        private static bool ProcessInput(string[] args)
        {
            if (args.Length < 2)
            {
                return false;
            }

            string entityName;
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

            List<string> parameters = new List<string>();
            for (var i = startIndex; i < args.Length; i++)
            {
                parameters.Add(args[i]);
            }

            if (parameters.Any())
            {
                entity = new EntityWithKey(entityName, parameters);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Executes the save entity command on the member called entity.
        /// </summary>
        private static void ExecuteSaveCommand()
        {
            TdsClient tdsClient = new TdsClient(path);

            List<EntityWithKey> entities = new List<EntityWithKey>();
            entities.Add(entity);

            try
            {
                tdsClient.SaveEntities(entities);
                Console.WriteLine("The given entity is saved.");
            }
            catch (Exception e)
            {
                if (e is EntityAlreadySavedException)
                {
                    string answer = string.Empty;
                    Console.WriteLine("The entity with the given keys has already been saved.");
                    while ((answer != "Y") && (answer != "N"))
                    {
                        Console.WriteLine("Overwrite? (Y/N)");
                        answer = Console.ReadLine();

                        switch (answer)
                        {
                            case "Y":
                                tdsClient.SaveEntities(entities, true);
                                Console.WriteLine("The given entity is saved.");
                                break;
                            case "N":
                                Console.WriteLine("Save aborted.");
                                break;
                            default:
                                Console.WriteLine("Unknown answer.");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine(e);
                }
            }
        }

        /// <summary>
        /// Main function, saves entity based on args
        /// </summary>
        static void Main(string[] args)
        {
            if (ProcessInput(args))
            {
                if (path == string.Empty)
                {
                    try
                    {
                        path = ConfigurationManager.AppSettings["TdsStoragePath"];
                    }
                    catch
                    {
                        Console.WriteLine("Corrupt App config. Invalid TdsStoragePath.");
                    }
                }

                Console.Write("Executing save on entity: " + entity.ToString());
                ExecuteSaveCommand();
            }
            else
            {
                Console.WriteLine("Too few or no arguments.");
                Console.WriteLine("Correct calls:");
                Console.WriteLine("    entityName param1 <param2 ..>");
                Console.WriteLine("    -path=targetPath entityName param1 <param2 ..>");
            }
            Console.ReadLine();
        }
    }
}
