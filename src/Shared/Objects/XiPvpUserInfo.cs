using Shared.Util;

namespace Shared.Objects
{
    public class XiPvpUserInfo : BinaryWriterExt.ISerializable
    {
        public ushort Level;
        public ushort Port;
        public uint Ip;


        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(Level);
            writer.Write(Port);
            writer.Write(Ip);
        }
    
        public static XiPvpUserInfo Deserialize(BinaryReaderExt reader) {
            return new XiPvpUserInfo() {
                Level = reader.ReadUInt16(),
                Port = reader.ReadUInt16(),
                Ip = reader.ReadUInt32()
            };
        }
    }
}
