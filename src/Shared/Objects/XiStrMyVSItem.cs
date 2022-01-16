using Shared.Util;


namespace Shared.Objects
{
    public class XiStrMyVSItem : BinaryWriterExt.ISerializable
    {
        public uint CarID;
        public int ItemState;
        public uint TableIdx;
        public uint InvenIdx;
        XiStrPlateName PlateName;
        public int Period;
        public int UpdateTime;
        public int CreateTime;


        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(CarID);
            writer.Write(ItemState);
            writer.Write(TableIdx);
            writer.Write(InvenIdx);
            writer.Write(PlateName);
            writer.Write(Period);
            writer.Write(UpdateTime);
            writer.Write(CreateTime);
        }

    }
}
