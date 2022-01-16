using Shared.Util;


namespace Shared.Objects
{
    public class XiStrFriend : BinaryWriterExt.ISerializable
    {
        public XiStrCharName CharName;
        public XiStrTeamName TeamName;
        public long Cid;
        public long TeamId;
        public long TeamMarkId;
        public int State;
        public XiStrLocation Location;
        public ushort Level;
        public ushort CurCarGrade;
        public uint Serial;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write((BinaryWriterExt.ISerializable)CharName);
            writer.Write(TeamName);
            writer.Write(Cid);
            writer.Write(TeamId);
            writer.Write(TeamMarkId);
            writer.Write(State);
            writer.Write(Location);
            writer.Write(Level);
            writer.Write(CurCarGrade);
            writer.Write(Serial);
        }

    }
}
