using Shared.Network;
using Shared.Util;
using Shared.Util.Commands;

namespace GameServer.Network.Handlers
{
    public class GameStream
    {
        [Packet(Packets.CmdGameStream)]
        public static void Handle(Packet packet)
        {
            var unknown1 = packet.Reader.ReadInt16();

            var message = packet.Reader.ReadUnicode();

            var args = ConsoleUtil.ParseLine(message);
            if (args.Count <= 0) return;
            var cmd = args[0];
            args.RemoveAt(0); // Remove command itself

            var command = GameServer.ChatCommands.GetCommand(cmd);

            if (command == null) return;

            if ((int)packet.Sender.User.Permission < command.RequiredPermission)
            {
                Log.Warning("User tried to use admin command: " + command.Name);
                return;
            }

            var res = command.Func(GameServer.Instance.Server, packet.Sender, cmd, args);
            if (res == CommandResult.InvalidArgument)
            {
                packet.Sender.SendChatMessage("Syntax: " + command.Usage);
            }
        }
    }
}