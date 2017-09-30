using System.IO;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_5266A0
    /// </summary>
    public class QuestStartAnswer : OutPacket
    {
        public uint TableIndex;
        public int FailNum;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.QuestStartAck);
        }
        
        public override int ExpectedSize() => 10;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(TableIndex);
                    bs.Write(FailNum);
                }
                return ms.ToArray();
            }
        }
    }
}