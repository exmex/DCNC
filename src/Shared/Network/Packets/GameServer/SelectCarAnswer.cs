using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_523EB0
    /// </summary>
    public class SelectCarAnswer : OutPacket
    {
        public Vehicle Vehicle = new Vehicle();
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.SelectCarAck);
        }
        
        public override int ExpectedSize() => 52;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Vehicle);
                }
                return ms.ToArray();
            }
        }
    }
}