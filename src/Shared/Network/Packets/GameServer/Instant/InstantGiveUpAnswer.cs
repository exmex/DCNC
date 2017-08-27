using System.IO;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class InstantGiveUpAnswer : OutPacket
    {
        public uint TableIndex;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.InstantGiveUpAck);
        }

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