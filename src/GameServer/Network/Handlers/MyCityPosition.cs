using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class MyCityPosition
    {
        [Packet(Packets.CmdMyCityPosition)]
        public static void Handle(Packet packet)
        {
            int channelId = packet.Reader.ReadInt32();
            //Console.WriteLine(packet.Reader.ReadUInt32());
            // SHORT CHANNELID
            // -> Gate ID!

            var character = packet.Sender.User.ActiveCharacter;
            packet.Sender.Send(new MyCityPositionAnswer()
            {
                City = character.City,
                LastChannel = channelId,
                Position = character.Position,
                PositionState = character.PosState
            }.CreatePacket());
        }
    }
}