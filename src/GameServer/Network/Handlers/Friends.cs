using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class Friends
    {
        [Packet(Packets.CmdFriendList)]
        public static void FriendList(Packet packet)
        {
            FriendListAnswerPacket friendListAnswerPacket = new FriendListAnswerPacket
            {
                TotalItemNum = 1,
                FriendList = new Friend[1]
            };
            friendListAnswerPacket.FriendList[1] = new Friend()
            {
                CharacterName = "TESTING",
                TeamName = "Staff",
                CharacterId = 1L,
                TeamId = 1L,
                TeamMarkId = 1L,
                State = 0,

                Serial = 0,
                LocationType = (char)41,
                ChannelId = (char)41,
                LocationId = 1,
                Level = 1,
                CurCarGrade = 1
            };
            friendListAnswerPacket.Send(packet.Sender);
        }

        [Packet(Packets.CmdFriendDel)]
        public static void FriendDel(Packet packet)
        {
            /*
            [Debug] - FriendDel: 000000: 54 00 45 00 53 00 54 00 49 00 4E 00 47 00 00 00  T · E · S · T · I · N · G · · ·
            000016: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · ·
            000032: 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · ·

            [Info] - Received FriendDel (id 235, 0xEB) on 11021. 
            */
            FriendList(packet);
            //var charName = packet.Reader.ReadUnicodeStatic(21);
        }

        [Packet(226)]
        public static void BlockAddByName(Packet packet)
        {
            /*
            [Debug] - 226: 000000: 31 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  1 · · · · · · · · · · · · · · ·
            000016: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · ·
            000032: 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · ·

            Wrong Packet Size. CMD(227) CmdLen: : 114, AnalysisSize: 4
            */

            var charName = packet.Reader.ReadUnicodeStatic(21);

            var ack = new Packet(227);
            ack.Writer.Write(1);
            ack.Writer.Write(0x40000); // or 262145

            // Friend
            ack.Writer.WriteUnicodeStatic("TESTING", 21); // Name
            ack.Writer.WriteUnicodeStatic("Staff", 13); // Team Name
            ack.Writer.Write(1L); // Cid
            ack.Writer.Write(1L); // TeamId
            ack.Writer.Write(1L); // TeamMarkId
            ack.Writer.Write(0); // State (0 = Blocked?)

            // StrLocation
            ack.Writer.Write((uint)0); // Serial
            ack.Writer.Write('A'); // LocType
            ack.Writer.Write('A'); // ChId
            ack.Writer.Write((ushort)1); // LocId

            ack.Writer.Write((ushort)1); // Level
            ack.Writer.Write((ushort)1); // CurCarGrade
            ack.Writer.Write((uint)0); // Serial

            packet.Sender.Send(ack);
        }

        [Packet(223)]
        public static void BlockDel(Packet packet)
        {
            
        }

        /*
         * 569 -> BlockList?
        struct XiBlockInfo
        {
          unsigned int m_Serial;
          int m_DueTime;
        }; 
        */
    }
}
