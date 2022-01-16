using Shared.Util;


namespace Shared.Objects
{
    public class XiStrLocation : BinaryWriterExt.ISerializable
    {
        public string LocType;
        public string ChId;
        public ushort LocId;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(LocType);
            writer.Write(ChId);
            writer.Write(LocId);
        }
    }
}
