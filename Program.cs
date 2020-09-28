using System;
using Microsoft.AnalysisServices.Tabular;

namespace PowerBIUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1 || !Int32.TryParse(args[0], out int portNum))
                {
                    throw new ArgumentException("Port number must be specified.");
                }

                Console.WriteLine($"Working with Power BI on port {portNum}");

                var server = new Server();
                server.Connect($"localhost:{portNum}");

                // System.Diagnostics.Debugger.Launch();

                var model = server.Databases[0].Model;
                foreach (var table in model.Tables)
                {
                    Console.WriteLine($"Table: {table.Name}");
                    foreach (var part in table.Partitions)
                    {
                        Console.WriteLine($"    Partition: {part.Name} ({part.SourceType})");
                        if (part.Source is MPartitionSource src)
                        {
                            Console.WriteLine(src.Expression);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType().FullName} - {ex.Message}");
            }
        }
    }
}
