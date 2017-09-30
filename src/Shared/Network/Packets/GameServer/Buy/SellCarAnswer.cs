using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_5211A0
    /// </summary>
    public class SellCarAnswer : OutPacket
    {
        public int CarId; // Fixed to int, since packetSize is 10, not 14!
        public int SellPrice;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.SellCarAck);
        }
        
        public override int ExpectedSize() => 10;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(CarId);
                    bs.Write(SellPrice);
                }
                return ms.ToArray();
            }
        }
    }
}