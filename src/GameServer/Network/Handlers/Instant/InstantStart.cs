using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class InstantStart
    {
        [Packet(Packets.CmdInstantStart)]
        public static void Handle(Packet packet)
        {
            var instantStartPacket = new InstantStartPacket(packet);

            var ack = new InstantStartAnswer {TableIndex = instantStartPacket.TableIndex};

            packet.Sender.Send(ack.CreatePacket());
        }
    }
}