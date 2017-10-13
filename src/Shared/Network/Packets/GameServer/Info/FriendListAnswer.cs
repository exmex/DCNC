using System;
using System.IO;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_52F8A0
    /// </summary>
    public class FriendListAnswer : OutPacket
    {
        public Friend[] FriendList = new Friend[0];
        //public int TotalItemNum;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.FriendListAck);
        }

        public override int ExpectedSize()
        {
            var totalFriends = Math.Min(FriendList.Length - 1, 12);
            return (112 * totalFriends) + 122;
        }

        // TODO: Serious logic mistake here. Telling the client to wait for another batch of friends, but never sending this
        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    var totalFriends = FriendList.Length;
                    if (totalFriends > 12)
                    {
                        var pktNum = totalFriends / 12 + 1; // Send maximum 12 friends per batch.
                        for (uint pktIdx = 0; pktIdx < pktNum; ++pktIdx)
                        {
                            uint sendItemCnt = 12;
                            if (pktIdx + 1 >= pktNum)
                                sendItemCnt = (uint) totalFriends - 12 * pktIdx;

                            bs.Write(sendItemCnt);
                            if (pktIdx < pktNum)
                                bs.Write(262145); // Send client that more packets coming after this one.
                            else
                                bs.Write(0x40000);
                            // Fill friends list
                            foreach (var friend in FriendList)
                            {
                                bs.WriteUnicodeStatic(friend.CharacterName, 21);
                                bs.WriteUnicodeStatic(friend.CrewName, 13);
                                bs.Write(friend.CharacterId);
                                bs.Write(friend.CrewId);
                                bs.Write(friend.CrewMarkId);
                                bs.Write(friend.State);

                                bs.Write(friend.Serial);
                                bs.Write(friend.LocationType);
                                bs.Write(friend.ChannelId);
                                bs.Write(friend.LocationId);
                                bs.Write(friend.Level);
                                bs.Write(friend.CurCarGrade);
                                bs.Write(friend.Serial);
                            }
                        }
                    }
                    else
                    {
                        bs.Write(totalFriends);
                        bs.Write(0x40000);
                        // Fill friends list
                        foreach (var friend in FriendList)
                        {
                            bs.WriteUnicodeStatic(friend.CharacterName, 21);
                            bs.WriteUnicodeStatic(friend.CrewName, 13);
                            bs.Write(friend.CharacterId);
                            bs.Write(friend.CrewId);
                            bs.Write(friend.CrewMarkId);
                            bs.Write(friend.State);

                            bs.Write(friend.Serial);
                            bs.Write(friend.LocationType);
                            bs.Write(friend.ChannelId);
                            bs.Write(friend.LocationId);
                            bs.Write(friend.Level);
                            bs.Write(friend.CurCarGrade);
                            bs.Write(friend.Serial);
                        }
                    }
                }
                return ms.ToArray();
            }

            /*ack.Writer.Write(1); // Friendlist size
            ack.Writer.Write(0x40000); // or 262145 //ListUpdate

            // Friend / XiStrFriend
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

            client.Send(ack);*/
        }
    }

    public class Friend // TODO: Move to Shared.Objects
    {
        public char ChannelId;
        public long CharacterId;
        public string CharacterName;
        public uint CurCarGrade;

        public ushort Level;
        public ushort LocationId;
        public char LocationType; // byte..?

        public uint Serial; // SessionId
        public int State; // FriendState (0 = Blocked?)
        public long CrewId;
        public long CrewMarkId;
        public string CrewName;
    }
}