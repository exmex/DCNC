using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Network;

namespace GameServer.Network.Handlers
{
    public class Friends
    {
        [Packet(Packets.CmdFriendList)]
        public static void FriendList(Packet packet) // TODO: Send actual data not just dummies
        {
            /*
            [Debug] - FriendList:
            [Info] - Received FriendList (id 230, 0xE6) on 11021.
            */
            var ack = new Packet(Packets.FriendListAck);
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
