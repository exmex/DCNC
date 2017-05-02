using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.AreaServer
{
    public class UdpCastTcsSignalAnswerPacket
    {
        public int Time;
        public int Signal;
        public int State;
        public Packet Send(Client client)
        {
            var ack = new Packet(Packets.UdpCastTcsSignalAck);
            ack.Writer.Write(Time);
            ack.Writer.Write(Signal);
            ack.Writer.Write(State);
            return ack; // <-- Weird hack since we don't have access to broadcast here.
        }
    }
}
