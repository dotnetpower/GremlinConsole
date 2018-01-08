namespace GremlinConsole
{
    using GremlinConsole.Graph;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    class Program
    { 
        static void Main(string[] args)
        {
            IConfigurationRoot builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string endpointUri = builder["cosmosDBConnection:endpoint"];
            string authKey = builder["cosmosDBConnection:authkey"];
            string databaseId = builder["cosmosDBConnection:databaseid"];
            string graphId = "Persons";

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("          \\,,,/");
            Console.WriteLine("          (o o)");
            Console.WriteLine(" -----oOOo-(3)-oOOo-----");
            Console.WriteLine("");
            Console.WriteLine(" hacked by moonchoi");
            Console.WriteLine("");
            
            Gremlin gremlin = new Gremlin(endpointUri, authKey, databaseId);

            try
            {
                Console.WriteLine("Connecting...");
                Task<List<dynamic>> task = Task.Run(async () => await gremlin.GremlinQuery<dynamic>(graphId, "g.V().Count()"));
                var test = task.Result;
                Console.WriteLine("Connected!");
            }
            catch
            {
                Console.WriteLine("check your endpointURI, authKey, databaseId");
                return;
            }

            Console.Write("gremlin>");

            string strLine = string.Empty;
            while (true)
            {
                string line = Console.ReadLine();

                try
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        switch (line)
                        {
                            case "help":
                                DisplayUsage();
                                break;
                            default:
                                Task<List<dynamic>> task = Task.Run(async () => await gremlin.GremlinQuery<dynamic>(graphId, line));
                                var result = task.Result;
                                string strJson = JsonConvert.SerializeObject(result);

                                Console.WriteLine(strJson);
                                Console.WriteLine();
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("type help");
                        Console.WriteLine();
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.Write("gremlin>");
                line = "";

            }
        }
        static void DisplayUsage()
        {
            Console.WriteLine(" addE\t\tAdds an edge between two vertices");
            Console.WriteLine(" addV\t\tAdds a vertex to the graph");
            Console.WriteLine(" and\t\tEnsures that all the traversals return a value");
            Console.WriteLine(" as\t\tA step modulator to assign a variable to the output of a step");
            Console.WriteLine(" by\t\tA step modulator used with group and order");
            Console.WriteLine(" coalesce\tReturns the first traversal that returns a result");
            Console.WriteLine(" constant\tReturns a constant value. Used with coalesce");
            Console.WriteLine(" count\t\tReturns the count from the traversal");
            Console.WriteLine(" dedup\t\tReturns the values with the duplicates removed");
            Console.WriteLine(" drop\t\tDrops the values (vertex/edge)");
            Console.WriteLine(" fold\t\tActs as a barrier that computes the aggregate of results");
            Console.WriteLine(" group\t\tGroups the values based on the labels specified");
            Console.WriteLine(" has\t\tUsed to filter properties, vertices, and edges. Supports hasLabel, hasId, hasNot, and has variants.");
            Console.WriteLine(" inject\t\tInject values into a stream");
            Console.WriteLine(" is\t\tUsed to perform a filter using a boolean expression");
            Console.WriteLine(" limit\t\tUsed to limit number of items in the traversal");
            Console.WriteLine(" local\t\tLocal wraps a section of a traversal, similar to a subquery");
            Console.WriteLine(" not\t\tUsed to produce the negation of a filter");
            Console.WriteLine(" optional\tReturns the result of the specified traversal if it yields a result else it returns the calling element");
            Console.WriteLine(" or\t\tEnsures at least one of the traversals returns a value");
            Console.WriteLine(" order\t\tReturns results in the specified sort order");
            Console.WriteLine(" path\t\tReturns the full path of the traversal");
            Console.WriteLine(" project\t\tProjects the properties as a Map");
            Console.WriteLine(" properties\tReturns the properties for the specified labels");
            Console.WriteLine(" range\t\tFilters to the specified range of values");
            Console.WriteLine(" repeat\t\tRepeats the step for the specified number of times. Used for looping");
            Console.WriteLine(" sample\t\tUsed to sample results from the traversal");
            Console.WriteLine(" select\t\tUsed to project results from the traversal");
            Console.WriteLine(" store\t\tUsed for non-blocking aggregates from the traversal");
            Console.WriteLine(" tree\t\tAggregate paths from a vertex into a tree");
            Console.WriteLine(" unfold\t\tUnroll an iterator as a step");
            Console.WriteLine(" union\t\tMerge results from multiple traversals");
            Console.WriteLine(" V\t\tIncludes the steps necessary for traversals between vertices and edges V, E, out, in,");
            Console.WriteLine(" \t\tboth, outE, inEbothE, outV, inV, bothV, and otherV for");
            Console.WriteLine(" where\t\tUsed to filter results from the traversal. Supports eq, neq, lt, lte, gt, gte, and between operators");

        }
    }
}
