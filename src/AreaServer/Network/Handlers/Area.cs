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
            TimeSyncPacket timeSyncPacket = new TimeSyncPacket(packet);
		    new TimeSyncAnswerPacket()
		    {
                GlobalTime = timeSyncPacket.LocalTime,
                SystemTick = 0
		    }.Send(packet.Sender);
		}

		[Packet(Packets.CmdAreaStatus)]
		public static void AreaStatus(Packet packet)
		{
            new AreaStatusAnswerPacket().Send(packet.Sender);
		}

		[Packet(Packets.CmdEnterArea)]
		public static void EnterArea(Packet packet)
		{
            EnterAreaPacket enterAreaPacket = new EnterAreaPacket(packet);

		    new EnterAreaAnswerPacket()
		    {
		        LocalTime = enterAreaPacket.LocalTime,
                SystemTick = 0,
                AreaId = enterAreaPacket.AreaId
		    }.Send(packet.Sender);

            //Log.WriteLine("Name: " + name);
            Log.Debug("Sessid: " + enterAreaPacket.SessionId);
			Log.Debug("LocalTime: " + enterAreaPacket.LocalTime);
			
		}
	}
}
