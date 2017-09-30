using System;
using System.Collections.Generic;
using Shared.Util.Commands;

namespace LobbyServer.Util
{
    public class LobbyConsoleCommands : ConsoleCommands
    {
        protected override CommandResult HandleConnections(string command, IList<string> args)
        {
            var i = 0;
            foreach (var client in LobbyServer.Instance.Server.GetClients())
            {
                Console.WriteLine($"{i} - {client.EndPoint}");
                i++;
            }
            return CommandResult.Okay;
        }
    }
}