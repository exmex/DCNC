using System;
using Shared.Util;

namespace LobbyServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
#if !DEBUG
            try
            {
#endif
                LobbyServer.Instance.Run();
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