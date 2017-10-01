using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers.Join
{
    public class CheckInGame
    {
        [Packet(Packets.CmdCheckInGame)]
        public static void Handle(Packet packet)
        {
            var checkInGamePacket = new CheckInGamePacket(packet);
            // TODO: Check packet here!
            var ack = new CheckInGameAnswer()
            {
                Result = 1
            };
            packet.Sender.Send(ack.CreatePacket());
        }
    }
}