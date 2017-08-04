using System;

namespace Shared.Network.AreaServer
{
    public class EnterAreaAnswerPacket
    {
        public uint AreaId;
        public uint LocalTime;

        public uint SystemTick;

        public void Send(Client client)
        {
            /*
			 * *(_WORD *)lpAckPkt = 563;
			 * *(_DWORD *)(lpAckPkt + 2) = lpMsg->m_Area;
			  *(_DWORD *)(lpAckPkt + 6) = 1;
			  *(_DWORD *)(lpAckPkt + 10) = lpMsg->m_nLocalTime;
			  *(_DWORD *)(lpAckPkt + 14) = GetSystemTick();
			*/

            var ack = new Packet(Packets.EnterAreaAck);
            ack.Writer.Write(AreaId);
            ack.Writer.Write(LocalTime);
            ack.Writer.Write(Environment.TickCount);

            /*ack.Writer.Write(LocalTime); // (Shouldn't this be third?)
            ack.Writer.Write(SystemTick); // System Tick (Shouldn't this be last?)
            ack.Writer.Write(AreaId); // (Shouldn't this be first?)
            ack.Writer.Write(1); // ?? (Shouldn't this be second?)
            ack.Writer.Write(new byte[2]); // Missing information for this one.*/
            client.Send(ack);
        }
    }
}