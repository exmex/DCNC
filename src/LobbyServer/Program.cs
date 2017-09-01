using System;
using System.Threading;
using Shared.Util;

namespace LobbyServer
{
    internal class Program
    {
        private static readonly Mutex Mutex = new Mutex(true, "{85637f23-235d-4dd9-8de1-210d78e323d9}");

        private static void Main(string[] args)
        {
#if !DEBUG
            try
            {
#endif
                if (Mutex.WaitOne(TimeSpan.Zero, true))
                {
                    LobbyServer.Instance.Run();
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