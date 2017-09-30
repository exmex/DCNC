using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_52A2D0
    /// </summary>
    public class BuyVisualItemThreadAnswer : OutPacket
    {
        /// <summary>
        /// Type 0 = BoxItem 
        /// Type 1 = Item 
        /// </summary>
        public int Type;
        public uint TableIndex;
        public uint CarId;
        // Inventory Id (Slot?)
        public int InventoryId;
        
        /// <summary>
        /// Length of time to buy
        /// 1 = 7 Days
        /// 2 = 30 Days
        /// 3 = 90 Days
        /// 4 = 0 Days
        /// 5 = Infinite Days
        /// </summary>
        public int Period;
        public int Mito;
        public int Hancoin;
        public int BonusMito;
        public int Mileage;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.BuyVisualItemThreadAck);
        }
        
        public override int ExpectedSize() => 38;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Type);
                    bs.Write(TableIndex);
                    bs.Write(CarId);
                    bs.Write(InventoryId);
                    bs.Write(Period);
                    bs.Write(Mito);
                    bs.Write(Hancoin);
                    bs.Write(BonusMito);
                    bs.Write(Mileage);
                }
                return ms.ToArray();
            }
        }
    }
}