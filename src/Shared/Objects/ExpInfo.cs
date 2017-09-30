using Shared.Util;

namespace Shared.Objects
{
    public struct ExpInfo : BinaryWriterExt.ISerializable
    {
        public long CurExp;
        public long NextExp;
        public long BaseExp;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(CurExp);
            writer.Write(NextExp);
            writer.Write(BaseExp);
        }
    }
}