using System.IO;
using System.Net.Configuration;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class QuestGiveUpAnswer : IOutPacket
    {
        public uint TableIndex;
        public byte Unknown1;
        
        public Packet CreatePacket()
        {
            var ack = new Packet(Packets.QuestGiveUpAck);
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
                    bs.Write(Unknown1);
                }
                return ms.GetBuffer();
            }
        }
    }
}