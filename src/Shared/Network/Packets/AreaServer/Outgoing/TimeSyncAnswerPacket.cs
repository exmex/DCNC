using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.AreaServer
{
    public class TimeSyncAnswerPacket
    {
        public uint GlobalTime;
        public uint SystemTick = 0;

        public void Send(uint packetId, Client client)
        {
            var ack = new Packet(Packets.UdpTimeSyncAck);
            ack.Writer.Write(packet.Reader.ReadUInt32()); // Relay?
            ack.Writer.Write(SystemTick); // System Tick.
            client.Send(ack);
        }
    }
}
