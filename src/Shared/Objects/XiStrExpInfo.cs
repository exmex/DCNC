using Shared.Util;


namespace Shared.Objects {

    public class XiStrExpInfo : BinaryWriterExt.ISerializable {
        public uint CurExp;
        public uint NextExp;
        public uint BaseExp;

        public void Serialize(BinaryWriterExt writer) {
            writer.Write(CurExp);
            writer.Write(NextExp);
            writer.Write(BaseExp);
        }

        public static XiStrExpInfo Deserialize(BinaryReaderExt reader) {
            return new XiStrExpInfo() {
                CurExp = reader.ReadUInt32(),
                NextExp = reader.ReadUInt32(),
                BaseExp = reader.ReadUInt32()
            };
        }
    }
}
