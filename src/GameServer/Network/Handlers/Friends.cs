using System.Collections.Generic;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class Friends
    {
        [Packet(Packets.CmdFriendList)]
        public static void FriendList(Packet packet)
        {
            List<Friend> friends = FriendModel.Retrieve(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId);
            FriendListAnswerPacket friendListAnswerPacket = new FriendListAnswerPacket
            {
                TotalItemNum = friends.Count,
                FriendList = friends.ToArray(), //new Friend[1]
            };
            /*friendListAnswerPacket.FriendList[0] = new Friend()
            {
                CharacterName = "TESTING",
                TeamName = "Staff",
                CharacterId = 1L,
                TeamId = 1L,
                TeamMarkId = 1L,
                State = 0,

                Serial = 0,
                LocationType = (char)1, // 1 => Area Not working.
                ChannelId = (char)0,
                LocationId = 1, // Area
                Level = 1,
                CurCarGrade = 1
            };*/
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
            var charName = packet.Reader.ReadUnicodeStatic(21);

            FriendModel.Delete(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId, charName);

            FriendList(packet);
        }

        [Packet(Packets.CmdFriendAddByName)]
        public static void FriendAddByName(Packet packet)
        {
            var charName = packet.Reader.ReadUnicodeStatic(21);

            //TODO: Send friend request instead of instantly adding him.
            if (FriendModel.AddByName(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId,
                charName))
            {
                FriendList(packet);
            }
        }

        [Packet(Packets.CmdBlockAddByName)]
        public static void BlockAddByName(Packet packet)
        {
            /*
            [Debug] - 226: 000000: 31 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  1 · · · · · · · · · · · · · · ·
            000016: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · ·
            000032: 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · ·

            Wrong Packet Size. CMD(227) CmdLen: : 114, AnalysisSize: 4
            */

            var charName = packet.Reader.ReadUnicodeStatic(21);

            if(FriendModel.AddByName(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId,
                charName, 'B'))
            {
                FriendList(packet);
            }
        }

        [Packet(Packets.CmdBlockDel)]
        public static void BlockDel(Packet packet)
        {
            var charName = packet.Reader.ReadUnicodeStatic(21);

            FriendModel.Delete(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId, charName);
        }

        [Packet(Packets.CmdSendMail)]
        public static void SendMail(Packet packet)
        {
            // TODO:
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
