using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_521AF0
    /// </summary>
    public class BuyItemAnswer : OutPacket
    {
        public int ItemId;
        public uint Quantity;
        public int Price;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.BuyItemAck);
        }
        
        public override int ExpectedSize() => 14;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(ItemId);
                    bs.Write(Quantity);
                    bs.Write(Price);
                }
                return ms.ToArray();
            }
        }
    }
}