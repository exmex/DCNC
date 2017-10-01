using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class InstantGoalPlace
    {
        [Packet(Packets.CmdInstantGoalPlace)]
        public static void Handle(Packet packet)
        {
            var instantGoalPlacePacket = new InstantGoalPlacePacket(packet);

            var ack = new InstantGoalPlaceAnswer
            {
                TableIndex = instantGoalPlacePacket.TableIndex,
                PlaceIndex = instantGoalPlacePacket.PlaceIndex,
                EXP = 0
            };

            packet.Sender.Send(ack.CreatePacket());
        }
    }
}