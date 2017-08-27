using System.IO;
using Shared.Util;

namespace Shared.Network
{
    public class OutPacket
    {
        public virtual Packet CreatePacket()
        {
            return null;
        }

        protected Packet CreatePacket(ushort packetId)
        {
            var ack = new Packet(packetId);
            ack.Writer.Write(GetBytes());
            return ack;
        }

        public virtual byte[] GetBytes()
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