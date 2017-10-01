using System;
using Shared.Network;
using Shared.Util;

namespace Shared.Objects
{
    /*
    struct __unaligned __declspec(align(1)) XiStrTeamInfo
    {
      __int64 TeamId;
      __int64 TeamMarkId;
      wchar_t TeamName[13];
      wchar_t TeamDesc[61];
      char TeamUrl[33];
      unsigned int CreateDate;
      unsigned int CloseDate;
      unsigned int BanishDate;
      char OwnChannel[24];
      char TeamState[2];
      unsigned int TeamRanking;
      unsigned int TeamPoint;
      unsigned int ChannelWinCnt;
      unsigned int MemberCnt;
      __int64 TeamTotalExp;
      __int64 TeamTotalMoney;
      unsigned int Version;
      __int64 OwnerId;
      __int64 LeaderId;
      XiStrCharName OwnerName;
      XiStrCharName LeaderName;
    };
    */
    /// <summary>
    /// TODO: Rename to Crew 
    /// </summary>
    public class Team : BinaryWriterExt.ISerializable
    {
        /// <summary>
        /// The DB Id of the team the user is in
        /// </summary>
        public long TeamId;
        
        /// <summary>
        /// The Image Id of the team 
        /// </summary>
        public long MarkId;
        
        /// <summary>
        /// The name of the team
        /// Unicode 13 (0xD) Chars
        /// </summary>
        public string Name;
        
        /// <summary>
        /// The description of the team
        /// Unicode 61 (0x3D) Chars
        /// </summary>
        public string Description;
        
        /// <summary>
        /// The URL of the Team
        /// ASCII 33 (0x21) Chars
        /// </summary>
        public string Url; // char 33
        
        /// <summary>
        /// The Unix Timestamp the team was created
        /// </summary>
        public uint CreateDate;
        
        /// <summary>
        /// The Unix Timestamp when the team was closed
        /// </summary>
        public uint CloseDate;
        
        /// <summary>
        /// The Unix Timestamp when the team was banished from game
        /// Could also be the unix timestamp since last user got banished
        /// </summary>
        public uint BanishDate;
        
        /// <summary>
        /// 
        /// ASCII 24 (0x18) Chars
        /// </summary>
        public string OwnChannel;
        
        /// <summary>
        /// The current state of the team
        /// TODO: Can be converted to two shorts
        /// Char 2 seems to be not used?
        /// ASCII 2 (0x2) Chars
        /// Char 0 =
        ///          0 = 
        ///          67 = Closed
        ///          68 = Deleted 
        /// Char 1 =
        /// 
        /// </summary>
        public string State; // char 2
        
        /// <summary>
        /// 
        /// </summary>
        public uint Ranking;
        
        /// <summary>
        /// 
        /// </summary>
        public uint Point;
        
        /// <summary>
        /// 
        /// </summary>
        public uint ChannelWinCnt;
        
        /// <summary>
        /// 
        /// </summary>
        public uint MemberCnt;
        
        /// <summary>
        /// Possibly the total EXP of all team members combined
        /// </summary>
        public long TotalExp;
        
        /// <summary>
        /// Possibly the total Mito of all team members combined
        /// </summary>
        public long TotalMoney;
        
        /// <summary>
        /// 
        /// Always 0 in the leaked server files
        /// </summary>
        public uint Version;
        
        /// <summary>
        /// 
        /// </summary>
        public long OwnerId;
        
        /// <summary>
        /// 
        /// </summary>
        public long LeaderId;
        
        /// <summary>
        /// Team Master character name
        /// Unicode 21 (0x15) Chars 
        /// </summary>
        public string OwnerName;
        
        /// <summary>
        ///
        /// Unicode 21 (0x15) Chars 
        /// </summary>
        public string LeaderName;

        public Team()
        {
            TeamId = -1L;
            MarkId = -1L;
            OwnerId = -1L;
            LeaderId = -1L;
        }

        // We're still missing ~209 bytes of data
        
        /// <summary>
        /// TODO: Missing: 293 Bytes!
        /// </summary>
        /// <param name="writer"></param>
        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(TeamId);
            writer.Write(MarkId);
            writer.WriteUnicodeStatic(Name, 13);
            writer.WriteUnicodeStatic(Description, 61);
            writer.WriteAsciiStatic(Url, 33);
            writer.Write(CreateDate);
            writer.Write(CloseDate);
            writer.Write(BanishDate);
            writer.WriteAsciiStatic(OwnChannel, 24);
            writer.WriteAsciiStatic(State, 2);
            writer.Write(Ranking);
            writer.Write(Point);
            writer.Write(ChannelWinCnt);
            writer.Write(MemberCnt);
            writer.Write(TotalExp);
            writer.Write(TotalMoney);
            writer.Write(Version);
            writer.Write(OwnerId);
            writer.Write(LeaderId);
            writer.WriteUnicodeStatic(OwnerName, 21);
            writer.WriteUnicodeStatic(LeaderName, 21);
            writer.Write(new byte[293]);
        }
    }
}