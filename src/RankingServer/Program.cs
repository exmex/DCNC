using System;
using Shared.Util;

namespace RankingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RankingServer.Instance.Run();
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "An exception occured while starting the server.");
                ConsoleUtil.Exit(1);
            }
        }
    }
}
