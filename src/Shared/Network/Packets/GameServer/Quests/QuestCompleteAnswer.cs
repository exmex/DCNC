using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_533B20
    /// </summary>
    public class QuestCompleteAnswer : OutPacket
    {
        public uint TableIndex;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.QuestCompleteAck);
        }
        
        public override int ExpectedSize() => 6;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(TableIndex);
                }
                return ms.ToArray();
            }
        }
    }
}