using System.IO;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class InstantStartAnswer : OutPacket
    {
        public uint TableIndex;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.InstantStartAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(TableIndex);
                    bs.Write(new byte[3]); // Missing?
                }
                return ms.ToArray();
            }
        }
    }
}