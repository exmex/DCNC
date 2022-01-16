using Shared.Util;


namespace Shared.Objects
{
    public class XiStrMyInstant : BinaryWriterExt.ISerializable
    {
        public uint TableIdx;
        public bool CheckPoint; //200 again unsure about bool 200

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(TableIdx);
            writer.Write(CheckPoint);
            
        }

    }
}
