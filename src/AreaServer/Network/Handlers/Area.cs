using Shared.Network;
using Shared.Network.AreaServer;
using Shared.Util;

namespace AreaServer.Network.Handlers
{
    public class Area
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

        [Packet(Packets.CmdAreaStatus)]
        public static void AreaStatus(Packet packet)
        {
            packet.Sender.Send(new AreaStatusAnswerPacket().CreatePacket());
        }

        [Packet(Packets.CmdEnterArea)]
        public static void EnterArea(Packet packet)
        {
            var enterAreaPacket = new EnterAreaPacket(packet);
            
            packet.Sender.Send(new EnterAreaAnswerOutPacket
            {
                LocalTime = enterAreaPacket.LocalTime,
                AreaId = enterAreaPacket.AreaId
            }.CreatePacket());

            //Log.WriteLine("Name: " + name);
            Log.Debug("Sessid: " + enterAreaPacket.SessionId);
            Log.Debug("LocalTime: " + enterAreaPacket.LocalTime);
        }
    }
}