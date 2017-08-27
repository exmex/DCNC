using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class BuyVisualItemThreadAnswer : OutPacket
    {
        // Type?
        public int Type;
        public uint TableIndex;
        public uint CarId;
        // Inventory Id (Slot?)
        public int InventoryId;
        // Period (1 = 7d, 2 = 30d, 4 = 00?, 5 = infinite)
        public int Period;
        public int Mito;
        public int Hancoin;
        public int BonusMito;
        public int Mileage;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.BuyVisualItemThreadAck);
        }

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