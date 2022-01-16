using Shared.Network;
using Shared.Network.Packets.GameServer.BattleZone;

namespace GameServer.Network.Handlers.BattleZone
{
    public  class UnregisterRoomObserver
    {
        [Packet(Packetss.CmdUnregisterRoomObserver)]
        public static void Handle(Packet packet)
        {
            var RoomJoinPacket = new UnregisterRoomObserverPacket(packet);
            packet.Sender.Send(new UnregisterRoomObserverAnswer()
            {
               

            }.CreatePacket());

        }
    }
}
