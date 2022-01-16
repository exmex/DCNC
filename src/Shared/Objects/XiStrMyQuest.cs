using Shared.Util;


namespace Shared.Objects
{
    public class XiStrMyQuest
    {
        
        public uint TableIdx;
        public uint State;
        public uint PlaceIdx;
        public ushort FailNum;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(TableIdx);
            writer.Write(State);
            writer.Write(PlaceIdx);
            writer.Write(FailNum);
        }

        public static XiStrMyQuest Deserialize(BinaryReaderExt reader)
        {
            return new XiStrMyQuest()
            {
                TableIdx = reader.ReadUInt32(),
                State = reader.ReadUInt32(),
                PlaceIdx = reader.ReadUInt32(),
                FailNum = reader.ReadUInt16()
            };
        }

    }
}
