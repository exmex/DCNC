using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class GetMyHancoinThread
    {
        [Packet(Packets.CmdGetMyHancoinThread)]
        public static void Handle(Packet packet)
        {
            var ack = new GetMyHancoinAnswer
            {
                Hancoins = packet.Sender.User.ActiveCharacter.Hancoin,
                Mileage = 100
            };

            packet.Sender.Send(ack.CreatePacket());
        }

    }
}