using Shared.Util;


namespace Shared.Objects
{
    public class XiStrMyArbeit : BinaryWriterExt.ISerializable
    {
        public uint TableIdx;
        public uint Serial;
        public uint CompleteNum;
        public bool CheckPoint; //20??? unsure about this

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(TableIdx);
            writer.Write(Serial);
            writer.Write(CompleteNum);
            writer.Write(CheckPoint);

        }
    }
}
