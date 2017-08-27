using System;
using System.IO;
using System.Runtime.CompilerServices;
using Shared.Util;

namespace Shared.Network.GameServer
{

    // TODO: Buy car still has not the correct structure.
    public class BuyCarAnswer : OutPacket
    {
        public int Id;
        public uint CarType;
        public int BaseColor;
        public int Grade;
        public int SlotType;
        public int AuctionCount;
        public float Fuel;
        public float Kilometer; // Per hour!?
        public uint Color;
        public float FuelCapacity;
        public float FuelEfficiency;
        public bool AuctionOn;
        public int Unknown1;
        public short Unknown2;
        public int Price;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.BuyCarAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Id); // ID
                    bs.Write(CarType);
                    bs.Write(BaseColor); // BaseColor?
                    bs.Write(Grade); // Grade?
                    bs.Write(SlotType); // SlotType?
                    bs.Write(AuctionCount); // AuctionCnt?
                    bs.Write(Fuel); // Mitron?
                    bs.Write(Kilometer); // Kmh?
                    bs.Write(Color);
                    bs.Write(FuelCapacity); // Mitron cap?
                    bs.Write(FuelEfficiency); // Mitron eff?
                    bs.Write(AuctionOn); // AuctionOn
                    bs.Write(Unknown1); // ?????
                    bs.Write(Unknown2); // ?????
                    bs.Write(Price); // Price
                    //ack.Writer.Write(Bumper);
                }
#if DEBUG
                return ms.ToArray();
#else
                return new byte[0];
#endif
            }
        }
    }
}