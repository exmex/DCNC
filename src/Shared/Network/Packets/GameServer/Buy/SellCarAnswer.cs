using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class SellCarAnswer : OutPacket
    {
        public ulong CarId;
        public int SellPrice;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.SellCarAck);
        }

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