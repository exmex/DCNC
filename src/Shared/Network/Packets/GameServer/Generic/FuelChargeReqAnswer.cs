using System.IO;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class FuelChargeReqAnswer : OutPacket
    {
        /*
        unsigned int CarId;
        __unaligned __declspec(align(1)) __int64 Pay;
        float DeltaFuel;
        __int64 Gold;
        float Fuel;
        float UnitPrice;
        float SaleUnitPrice;
        float FuelCapacity;
        float FuelEfficiency;
        int SaleFlag;
        */
        public uint CarId;

        public float DeltaFuel;
        public float DiscountedSaleUnitPrice;
        public float Fuel;
        public float FuelCapacity;
        public float FuelEfficiency;
        public long Gold;
        public long Pay;
        public int SaleFlag;
        public float SaleUnitPrice;

        public override Packet CreatePacket()
        {
            var packet = new Packet(Packets.FuelChargeReqAck);
            packet.Writer.Write(GetBytes());
            return packet;
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(CarId);
                    bs.Write(Pay);
                    bs.Write(DeltaFuel);
                    bs.Write(Gold);
                    bs.Write(Fuel);
                    bs.Write(SaleUnitPrice); // Mito Price per liter
                    bs.Write(
                        DiscountedSaleUnitPrice); // Discounted Price per liter (Normal channel, major channel is free?)
                    bs.Write(FuelCapacity);
                    bs.Write(FuelEfficiency);

                    bs.Write(SaleFlag); // 1 = Item Discount 50%
                    bs.Write(new byte[2]); // Not sure.
                }
                return ms.GetBuffer();
            }
        }
    }
}