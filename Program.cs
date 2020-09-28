using System;

namespace PowerBIUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if(args.Length < 1 || !Int32.TryParse(args[0], out int portNum))
                {
                    throw new ArgumentException("Port number must be specified.");
                }

                Console.WriteLine($"Working with Power BI on port {portNum}");

            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.GetType().FullName} - {ex.Message}");
            }
        }
    }
}
