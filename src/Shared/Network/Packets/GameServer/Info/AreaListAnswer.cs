using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class Area
    {
        public int AreaId;
        public int CurrentPlayers;
        public int MaxPlayers;
        public int ChannelState;
        public float Tax; // Tax?!
        
        public long TeamId;
        public long TeamMarkId;
        public string TeamName;
        public uint Ranking;
        public uint Point;
        public uint WinCount;
        public int MemberCount;
        public long OwnerId;
        public string OwnerName;
        public long TotalExp;
    };
    
    /// <summary>
    /// sub_53B740
    /// </summary>
    public class AreaListAnswer : OutPacket
    {
        public Area[] Areas = new Area[0];
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.AreaListAck);
        }
        
        public override int ExpectedSize() => (137 * Areas.Length-1) + 143;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Areas.Length-1);
                    foreach (var area in Areas)
                    {
                        bs.Write(area.AreaId);
                        bs.Write(area.CurrentPlayers);
                        bs.Write(area.MaxPlayers);
                        bs.Write(area.ChannelState);
                        bs.Write(area.Tax);
                        bs.Write(area.TeamId);
                        bs.Write(area.TeamMarkId);
                        bs.WriteUnicodeStatic(area.TeamName, 13);
                        bs.Write(area.Ranking);
                        bs.Write(area.Point);
                        bs.Write(area.WinCount);
                        bs.Write(area.MemberCount);
                        bs.Write(area.OwnerId);
                        bs.WriteUnicodeStatic(area.OwnerName, 21);
                        bs.Write(area.TotalExp);
                        bs.Write((long)0); // ??????
                    }
                }
                return ms.ToArray();
            }
            /*
            var ack = new Packet(Packets.AreaListAck);
            ack.Writer.Write((uint) 10);
            for (var k = 0; k < 10; ++k)
            {
                ack.Writer.Write(k); // AreaId
                ack.Writer.Write(0); // Current?
                ack.Writer.Write(100); // Max?
                ack.Writer.Write(1); // ChannelState
                ack.Writer.Write((float) 0); // Tax?

                ack.Writer.Write((long) 0); // teamID?
                ack.Writer.Write((long) 0); // teamMarkID
                ack.Writer.WriteUnicodeStatic("Staff", 13); // TeamName
                ack.Writer.Write((uint) 0); // Ranking
                ack.Writer.Write((uint) 0); // Point
                ack.Writer.Write((uint) 0); // WinCnt
                ack.Writer.Write(20); // Membercnt
                ack.Writer.Write((long) 1); // OwnerId
                ack.Writer.WriteUnicodeStatic("Administrator", 21); // OwnerName
                ack.Writer.Write((long) 0); // TotalExp
                ack.Writer.Write((long) 0); // ????????
            }
            */
        }
    }
}