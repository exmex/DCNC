using System;
using System.Threading;
using Shared.Util;

namespace RankingServer
{
    internal class Program
    {
        private static readonly Mutex Mutex = new Mutex(true, "{b38bcc22-7671-4a73-8a4f-07fca96db5cf}");

        private static void Main(string[] args)
        {
#if !DEBUG
            try
            {
#endif
                if (Mutex.WaitOne(TimeSpan.Zero, true))
                {
                    RankingServer.Instance.Run();
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