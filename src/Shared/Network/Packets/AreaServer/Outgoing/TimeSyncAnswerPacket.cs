using System.IO;
using Shared.Util;

namespace Shared.Network.AreaServer
{
    /// <summary>
    /// sub_5C950
    /// </summary>
    public class TimeSyncAnswerPacket : OutPacket
    {
        public uint GlobalTime;
        public uint SystemTick = 0;

        public override Packet CreatePacket()
        {
            var ack = new Packet(Packets.UdpTimeSyncAck);
            ack.Writer.Write(GetBytes());
            /*
            ack.Writer.Write(GlobalTime); // Relay?
            ack.Writer.Write(SystemTick); // System Tick.
            */
            return ack;
        }

        public override int ExpectedSize() => 10; 

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(GlobalTime);
                    bs.Write(SystemTick);
                }
                return ms.ToArray();
            }
        }
    }
}