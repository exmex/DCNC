using System;
using Shared.Util;

namespace LobbyServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                LobbyServer.Instance.Run();
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "An exception occured while starting the server.");
                ConsoleUtil.Exit(1);
            }
        }
    }
}