using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class JoinAreaAnswer : OutPacket
    {
        public int AreaId;
        public int Result;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.JoinAreaAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(AreaId);
                    bs.Write(Result);
                }
                return ms.GetBuffer();
            }
        }

        /*
        var ack = new Packet(Packets.JoinAreaAck);
        ack.Writer.Write(areaId); // AreaId
        ack.Writer.Write(1); // Result
        packet.Sender.Send(ack);
        */
    }
}