using Shared.Util;


namespace Shared.Objects
{
    public class XiStrVisualItemBuyUnit : BinaryWriterExt.ISerializable
    {
        public long Gid;
        public uint TableIdx;
        public uint BuyTime;
        public uint UseTime;
        public uint GetType;
        public uint GoldType;
        public int Period;
        public uint Hancoin;
        public uint Mito;
        public uint Mileage;
        public int State;
        public XiStrCharName DstName;
        public XiStrPlateName Data;
        public XiStrGiftMsg GiftMsg;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(Gid);
            writer.Write(TableIdx);
            writer.Write(BuyTime);
            writer.Write(UseTime);
            writer.Write(GetType);
            writer.Write(GoldType);
            writer.Write(Period);
            writer.Write(Hancoin);
            writer.Write(Mito);
            writer.Write(Mileage);
            writer.Write(State);
            writer.Write((BinaryWriterExt.ISerializable)DstName);
            writer.Write(Data);
            writer.Write(GiftMsg);
            
        }

    }
}
