using System.IO;
using Microsoft.SqlServer.Server;
using Shared.Util;

namespace Shared.Network.AreaServer
{
    public class UdpCastTcsSignalAnswerPacket : OutPacket
    {
        public int Signal;
        public int State;
        public int Time;

        public override Packet CreatePacket()
        {
            var ack = new Packet(Packets.UdpCastTcsSignalAck);
            ack.Writer.Write(GetBytes());
            /*ack.Writer.Write(Time);
            ack.Writer.Write(Signal);
            ack.Writer.Write(State);*/
            return ack;
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Time);
                    bs.Write(Signal);
                    bs.Write(State);
                }
                return ms.GetBuffer();
            }
        }
    }
}