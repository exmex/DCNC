using System;
using System.Collections.Generic;
using Shared.Network;
using Shared.Util.Commands;

namespace GameServer.Util
{
    public class GameConsoleCommands : ConsoleCommands
    {
        public GameConsoleCommands()
        {
        }

        protected override CommandResult HandleSendPkt(string command, IList<string> args)
        {
            short res;
            if(args.Count < 2)
                return CommandResult.Fail;
            if (!short.TryParse(args[1], out res))
                return CommandResult.InvalidArgument;

            var packet = new Packet(Convert.ToUInt16(args[1]));
            GameServer.Instance.Server.Broadcast(packet);
            return CommandResult.Okay;
        }
    }
}
