using System;
using System.IO;
using System.Linq;
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

                var outputPath = Path.Join(Directory.GetCurrentDirectory(), "output");
                if(!Directory.Exists(outputPath))
                {
                    throw new DirectoryNotFoundException("Output directory not found.");
                }

                Console.WriteLine($"Working with Power BI on port {portNum}");
                Console.WriteLine($"Writing to {outputPath}");

                var server = new Server();
                server.Connect($"localhost:{portNum}");

                // System.Diagnostics.Debugger.Launch();

                var model = server.Databases[0].Model;
                model.Tables
                    .Select(table => (Table: table, Partition: table.Partitions.FirstOrDefault()))
                    .Where(t => t.Partition != null && t.Partition.Source is MPartitionSource)
                    .Select(t => (t.Table, t.Partition, Source: t.Partition.Source as MPartitionSource))
                    .ToList()
                    .ForEach(t =>
                    {
                        var expr = t.Source.Expression;
                        if (!string.IsNullOrWhiteSpace(expr))
                        {
                            Console.WriteLine($"  - {t.Table.Name}");
                            var fileName = Path.Join(outputPath, $"{t.Table.Name}.m");
                            File.WriteAllText(fileName, expr);

                            // expr = $"// {DateTime.Now.ToString()}\n" + expr;
                            // t.Source.Expression = expr;
                            // // t.Partition.RequestRefresh(RefreshType.Full);
                            // // t.Table.RequestRefresh(RefreshType.Full);
                        }
                    });
                // model.SaveChanges();
                // model.RequestRefresh(RefreshType.Full);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType().FullName} - {ex.Message}");
            }
        }
    }
}
