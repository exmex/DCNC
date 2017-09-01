using Shared.Util;

namespace Shared.Objects
{
    public class XiStrCarInfo : BinaryWriterExt.ISerializable
    {
        public uint CarID;
        public uint CarType;
        public uint BaseColor;
        public uint Grade;
        public uint SlotType;
        public uint AuctionCnt;
        public float Mitron;
        public float Kmh;
        public uint Color;
        public float MitronCapacity;
        public float MitronEfficiency;
        public bool AuctionOn;
        
        public void Serialize(BinaryWriterExt writer)
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