using System.IO;
using System.Net.Configuration;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class QuestGiveUpAnswer : OutPacket
    {
        public uint TableIndex;
        public byte Unknown1;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.QuestGiveUpAck);
        }

        public override byte[] GetBytes()
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