using Shared.Network;
using Shared.Network.AreaServer;

namespace AreaServer.Network.Handlers
{
    public static class AreaStatus
    {
        [Packet(Packets.CmdAreaStatus)]
        public static void Handle(Packet packet)
        {
            packet.Sender.Send(new AreaStatusAnswerPacket()
            {
                UserCount = new uint[100],
            }.CreatePacket());
        }
    }
}