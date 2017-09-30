using System;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Util;

namespace GameServer.Network.Handlers
{
    public class Friends
    {
        [Packet(Packets.CmdFriendList)]
        public static void FriendList(Packet packet)
        {
            var friends = FriendModel.Retrieve(GameServer.Instance.Database.Connection,
                packet.Sender.User.ActiveCharacterId);
            if (friends.Count > 12)
            {
                var pktNum = friends.Count / 12 + 1; // Send maximum 12 friends per batch.
                for (uint pktIdx = 0; pktIdx < pktNum; ++pktIdx)
                {
                    var ack = new Packet(Packets.FriendListAck);
                    int sendItemCnt = 12;
                    if (pktIdx + 1 >= pktNum)
                        sendItemCnt = (int)(friends.Count - 12 * pktIdx);

                    ack.Writer.Write(sendItemCnt);
                    if (pktIdx < pktNum)
                        ack.Writer.Write((uint)262145); // Send client that more packets coming after this one.
                    else
                        ack.Writer.Write((uint)0x40000);
                    // Fill friends list
                    foreach (var friend in friends)
                    {
                        ack.Writer.WriteUnicodeStatic(friend.CharacterName, 21, true);
                        ack.Writer.WriteUnicodeStatic(friend.TeamName, 13, true);
                        ack.Writer.Write(friend.CharacterId);
                        ack.Writer.Write(friend.TeamId);
                        ack.Writer.Write(friend.TeamMarkId);
                        ack.Writer.Write(friend.State);
                    
                        ack.Writer.Write(friend.LocationType);
                        ack.Writer.Write(friend.ChannelId);
                        ack.Writer.Write(friend.LocationId);
                        ack.Writer.Write(friend.Level);
                        ack.Writer.Write(friend.CurCarGrade);
                        ack.Writer.Write(friend.Serial);
                    }
                    packet.Sender.Send(ack);
                }
            }
            else
            {
                var ack = new Packet(Packets.FriendListAck);
                ack.Writer.Write(friends.Count);
                ack.Writer.Write((uint)0x40000);
                // Fill friends list
                foreach (var friend in friends)
                {
                    ack.Writer.WriteUnicodeStatic(friend.CharacterName, 21, true);
                    ack.Writer.WriteUnicodeStatic(friend.TeamName, 13, true);
                    ack.Writer.Write(friend.CharacterId);
                    ack.Writer.Write(friend.TeamId);
                    ack.Writer.Write(friend.TeamMarkId);
                    ack.Writer.Write(friend.State);
                    
                    ack.Writer.Write(friend.LocationType);
                    ack.Writer.Write(friend.ChannelId);
                    ack.Writer.Write(friend.LocationId);
                    ack.Writer.Write(friend.Level);
                    ack.Writer.Write(friend.CurCarGrade);
                    ack.Writer.Write(friend.Serial);
                }
                packet.Sender.Send(ack);
            }
            var friendListAnswerPacket = new FriendListAnswer
            {
                FriendList = friends.ToArray() //new Friend[1]
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
                FriendList(packet);
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

            if (FriendModel.AddByName(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId,
                charName, 'B'))
                FriendList(packet);
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
            #if !DEBUG
            Log.Unimplemented("Not implemented");
            #endif
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