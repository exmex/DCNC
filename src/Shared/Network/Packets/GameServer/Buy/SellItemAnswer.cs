using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_5283E0
    /// </summary>
    public class SellItemAnswer : OutPacket
    {
        public uint TableIndex;
        public uint Quantity;
        public uint Money;
        public uint Slot;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.SellItemAck);
        }
        
        public override int ExpectedSize() => 18;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(TableIndex);
                    bs.Write(Quantity);
                    bs.Write(Money);
                    bs.Write(Slot);
                }
                return ms.ToArray();
            }
        }
    }
}