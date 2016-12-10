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
            Add("traffic", "Arbitrary traffic", (command, args) =>
            {
                var packet = new Packet(547);
                ushort carId = 1;
                if (args.Count > 1)
                {
                    carId = ushort.Parse(args[1]);
                }
                packet.Writer.Write(carId); // TCarId
                packet.Writer.Write((ushort)1); // Owner
                packet.Writer.Write((ushort)1); // Attr
                packet.Writer.Write((ushort)0); // Path

                // Pos
                packet.Writer.Write(1050.522f); // X
                packet.Writer.Write(-969.07f); // Y
                packet.Writer.Write(49.054f); // Z
                packet.Writer.Write(-1.783f); // W

                float x = 0.0f, y = 0.0f;
                if (args.Count > 2)
                {
                    x = float.Parse(args[2]);
                    y = float.Parse(args[3]);
                }

                // Velo
                packet.Writer.Write(x); // X
                packet.Writer.Write(y); // Y
                packet.Writer.Write(0.0f); // Z
                packet.Writer.Write(0.0f); // W

                packet.Writer.Write(0); // owntime
                packet.Writer.Write(0); // global time
                packet.Writer.Write(0); // freedtime 

                GameServer.Instance.Server.Broadcast(packet);

                return CommandResult.Okay;
            });
        }

        protected override CommandResult HandleSendPkt(string command, IList<string> args)
        {
            short res;
            int res2;
            if(args.Count < 2)
                return CommandResult.Fail;
            if (!short.TryParse(args[1], out res))
                return CommandResult.InvalidArgument;

            if (!int.TryParse(args[2], out res2) || res2 > 256)
                return CommandResult.InvalidArgument;

            var packet = new Packet(Convert.ToUInt16(args[1]));
            packet.Writer.Write(new byte[res2]);
            GameServer.Instance.Server.Broadcast(packet);
            return CommandResult.Okay;
        }
    }
}
