using Shared.Network;

namespace Shared.Objects
{
    public class Vehicle : ISerializable
    {
        public uint AuctionCnt;
        public bool AuctionOn;
        public uint BaseColor;
        public uint CarID;
        public uint CarType;
        public uint Color;

        public uint Color2;
        public uint Grade;
        public float Kmh;
        public float Mitron;
        public float MitronCapacity;

        public float MitronEfficiency;
        public bool SBBOn;
        public uint SlotType;


        public void Serialize(PacketWriter writer)
        {
            writer.Write(CarID);
            writer.Write(CarType);
            writer.Write(BaseColor);
            writer.Write(Grade);
            writer.Write(SlotType);
            writer.Write(AuctionCnt);
            writer.Write(Mitron);
            writer.Write(Kmh);
            writer.Write(Color);
            writer.Write(MitronCapacity);
            writer.Write(MitronEfficiency);
            writer.Write(AuctionOn);
        }
    }
}