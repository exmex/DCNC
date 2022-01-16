using Shared.Util;


namespace Shared.Objects
{
    public class XiRankUnit : BinaryWriterExt.ISerializable
    {
        public uint Time;
        public int Rank;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(Time);
            writer.Write(Rank);
            

        }

    }
}
