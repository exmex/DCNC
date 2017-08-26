using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class QuestCompleteAnswer : IOutPacket
    {
        public uint TableIndex;
        
        public Packet CreatePacket()
        {
            var ack = new Packet(Packets.QuestCompleteAck);
            ack.Writer.Write(GetBytes());
            return ack;
        }

        public byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(TableIndex);
                }
                return ms.GetBuffer();
            }
        }
    }
}