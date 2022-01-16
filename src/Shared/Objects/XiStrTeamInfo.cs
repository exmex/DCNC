using Shared.Util;


namespace Shared.Objects
{
    public class XiStrTeamInfo
    {
        public long TeamId;
        public long TeamMarkId;
        public string TeamName;// wchar_t 13
        public string TeamDesc;//wchar_t 61
        public char TeamUrl;//char 33
        public uint CreateDate;
        public uint CloseDate;
        public uint BanishDate;
        public char OwnChannel;//24
        public char TeamState;//2
        public uint TeamRanking;
        public uint TeamPoint;
        public uint ChannelWinCnt;
        public uint MemberCnt;
        public long TeamTotalExp;
        public long TeamTotalMoney;
        public uint Version;
        public long OwnerId;
        public long LeaderId;
        XiStrCharName OwnerName;
        XiStrCharName LeaderName;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(TeamId);
            writer.Write(TeamMarkId);
            writer.Write(TeamName);
            writer.Write(TeamDesc);
            writer.Write(TeamUrl);
            writer.Write(CreateDate);
            writer.Write(CloseDate);
            writer.Write(BanishDate);
            writer.Write(OwnChannel);
            writer.Write(TeamState);
            writer.Write(TeamRanking);
            writer.Write(TeamPoint);
            writer.Write(ChannelWinCnt);
            writer.Write(MemberCnt);
            writer.Write(TeamTotalExp);
            writer.Write(TeamTotalMoney);
            writer.Write(Version);
            writer.Write(OwnerId);
            writer.Write(LeaderId);
            writer.Write((BinaryWriterExt.ISerializable)OwnerName);
            writer.Write((BinaryWriterExt.ISerializable)LeaderName);
        }


    }
}
