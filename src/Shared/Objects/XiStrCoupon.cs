using Shared.Util;


namespace Shared.Objects
{
    public class XiStrCoupon : BinaryWriterExt.ISerializable
    {
        public int Type;
        public uint StampIdx;
        public uint State;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(Type);
            writer.Write(StampIdx);
            writer.Write(State);
        }

    }
}
