using System;
using System.Threading;
using Shared.Util;

namespace AreaServer
{
    internal class Program
    {
        private static readonly Mutex Mutex = new Mutex(true, "{1b8e2707-a8f1-4ebc-b1ee-99693bfbc6ba}");

        private static void Main(string[] args)
        {
#if !DEBUG
            try
            {
#endif
                if (Mutex.WaitOne(TimeSpan.Zero, true))
                {
                    AreaServer.Instance.Run();
                    Mutex.ReleaseMutex();
                }
                else
                {
                    Console.WriteLine("Server already running!");
                    Console.ReadKey();
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "An exception occured while running the server.");

                Console.WriteLine("Press any key to exit");
                ConsoleUtil.Exit(1);
            }
#endif
        }
    }
}