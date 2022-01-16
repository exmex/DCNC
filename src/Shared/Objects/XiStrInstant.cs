using Shared.Util;


namespace Shared.Objects
{
    public class XiStrInstant : BinaryWriterExt.ISerializable
    {
        public uint nID;
        public uint nMinLevel;
        public uint nMaxLevel;
        public string Summary; //256
        public string Dialog; //2048
        public bool bRepeat;
        public float Percent;
        public int Category;
        public int Type;
        public uint Value;
        public XiStrItem MissionItemPtr;
        public int RewardExp;
        public int RewardMoney;
        public string RewardItemGroup; //56
        public int FinalExp;
        public int MaxPost;
        public XiStrIcon GivePostPtr;
        public XiStrIcon MovePostPtr; //200
        //public ItemGroup ItemGroupPtr; TODO need to create the object


        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(nID);
            writer.Write(nMinLevel);
            writer.Write(nMaxLevel);
            writer.Write(Summary);
            writer.Write(Dialog);
            writer.Write(bRepeat);
            writer.Write(Percent);
            writer.Write(Category);
            writer.Write(Type);
            writer.Write(Value);
            writer.Write((BinaryWriterExt.ISerializable)MissionItemPtr);
            writer.Write(RewardExp);
            writer.Write(RewardMoney);
            writer.Write(RewardItemGroup);
            writer.Write(FinalExp);
            writer.Write(MaxPost);
            writer.Write((BinaryWriterExt.ISerializable)GivePostPtr);
            writer.Write((BinaryWriterExt.ISerializable)MovePostPtr);
            //writer.Write(ItemGroupPtr); TODO
            

        }
    }
}
