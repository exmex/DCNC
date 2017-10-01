using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers.Social
{
    public class FriendList
    {
        [Packet(Packets.CmdFriendList)]
        public static void Handle(Packet packet)
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
    }
}