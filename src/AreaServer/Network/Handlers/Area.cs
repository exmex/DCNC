using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Network;
using Shared.Util;

namespace AreaServer.Network.Handlers
{
	public class Area
	{
		[Packet(Packets.CmdUdpTimeSync)]
		public static void TimeSync(Packet packet)
		{
			var ack = new Packet(Packets.UdpTimeSyncAck);
			ack.Writer.Write(packet.Reader.ReadUInt32()); // Relay?
			ack.Writer.Write(0); // System Tick.
			packet.Sender.Send(ack);
		}

		[Packet(Packets.CmdAreaStatus)]
		public static void AreaStatus(Packet packet)
		{
			/*
			for ( i = 0; i < 100; ++i )
			{
			  pArea = BS_AreaGet(i);
			  if ( pArea )
				lpAckMsg->m_UserCnt[i] = pArea->m_member.m_nCount;
			  else
				lpAckMsg->m_UserCnt[i] = 0;
			}
			*/
			var ack = new Packet(Packets.AreaStatusAck);
			for (var i = 0; i < 100; ++i)
			{
				ack.Writer.Write(0);
			}
			packet.Sender.Send(ack);
		}

		[Packet(Packets.CmdEnterArea)]
		public static void EnterArea(Packet packet)
		{
			/*
			 * *(_WORD *)lpAckPkt = 563;
			 * *(_DWORD *)(lpAckPkt + 2) = lpMsg->m_Area;
			  *(_DWORD *)(lpAckPkt + 6) = 1;
			  *(_DWORD *)(lpAckPkt + 10) = lpMsg->m_nLocalTime;
			  *(_DWORD *)(lpAckPkt + 14) = GetSystemTick();
			*/
			var sessionId = packet.Reader.ReadInt16(); // Serial / SessionId
			var username = packet.Reader.ReadUnicode(); // Name?
			username = username.Substring(username.Length - 1);
			var areaId = packet.Reader.ReadUInt32(); // Area ID
			var localTime = packet.Reader.ReadUInt32(); // Local Time
			var groupid = packet.Reader.ReadUInt32(); // GroupID

			//Log.WriteLine("Name: " + name);
			Log.Debug("Sessid: " + sessionId);
			Log.Debug("LocalTime: " + localTime);
			var ack = new Packet(Packets.EnterAreaAck);
			//ack.Writer.Write(0);
			ack.Writer.Write(localTime);
			ack.Writer.Write(0); // System Tick
			ack.Writer.Write(areaId);
			ack.Writer.Write(1); // ??
			ack.Writer.Write(new byte[2]); // Missing information for this one.
			packet.Sender.Send(ack);
		}
	}
}
