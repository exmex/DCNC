using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class InstantGiveUp
    {
        [Packet(Packets.CmdInstantGiveUp)]
        public static void Handle(Packet packet)
        {
            var instantGiveUpPacket = new InstantGiveUpPacket(packet);

            var ack = new InstantGiveUpAnswer {TableIndex = instantGiveUpPacket.TableIndex};

            packet.Sender.Send(ack.CreatePacket());
        }
    }
}