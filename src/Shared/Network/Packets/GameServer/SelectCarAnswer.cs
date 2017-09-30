using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class SelectCarAnswer : OutPacket
    {
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.SelectCarAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                }
                return ms.ToArray();
            }
        }
    }
}