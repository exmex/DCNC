using System;
using System.IO;
using System.Threading;
using Shared.Util;

namespace GameServer
{
    internal class Program
    {
        private static readonly Mutex Mutex = new Mutex(true, "{9108d095-21d1-4378-abfc-67f2ecbdac28}");
        
        private static void Main(string[] args)
        {
#if !DEBUG
            try
            {
#endif
                if (Mutex.WaitOne(TimeSpan.Zero, true))
                {
                    GameServer.Instance.Run();
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