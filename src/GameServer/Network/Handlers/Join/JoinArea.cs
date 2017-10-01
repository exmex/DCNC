using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers.Join
{
    public class JoinArea
    {
        [Packet(Packets.CmdJoinArea)]
        public static void Handle(Packet packet)
        {
            var joinAreaPacket = new JoinAreaPacket(packet);
            packet.Sender.Send(new JoinAreaAnswer()
            {
                AreaId = joinAreaPacket.AreaId,
                Result = 1,
            }.CreatePacket());
        }
    }
}