using Shared.Network;
using Shared.Network.AreaServer;

namespace AreaServer.Network.Handlers
{
    public static class General
    {
        [Packet(Packets.CmdUdpTimeSync)]
        public static void TimeSync(Packet packet)
        {
            var timeSyncPacket = new TimeSyncPacket(packet);
            packet.Sender.Send(new TimeSyncAnswerPacket
            {
                GlobalTime = timeSyncPacket.LocalTime,
                SystemTick = 0
            }.CreatePacket());
        }
    }
}