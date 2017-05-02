using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Shared.Network.GameServer
{
    public class FriendListAnswerPacket
    {
        public int TotalItemNum;

        public Friend[] FriendList;

        public void Send(Client client) // TODO: Send actual data not just dummies
        {
            var ack = new Packet(Packets.FriendListAck);

            int pktNum = TotalItemNum / 12 + 1; // Send maximum 12 friends per batch.
            for (uint pktIdx = 0; pktIdx < pktNum; ++pktIdx)
            {
                uint SendItemCnt = 12;
                if (pktIdx + 1 == pktNum)
                    SendItemCnt = (uint)TotalItemNum - 12 * pktIdx;
                
                ack.Writer.Write(SendItemCnt);
                if(pktIdx < pktNum)
                    ack.Writer.Write(262145); // Send client that more packets coming after this one.
                else
                    ack.Writer.Write(0x40000);
                // Fill friends list
                foreach (var friend in FriendList)
                {
                    ack.Writer.WriteUnicodeStatic(friend.CharacterName, 21);
                    ack.Writer.WriteUnicodeStatic(friend.TeamName, 13);
                    ack.Writer.Write(friend.CharacterId);
                    ack.Writer.Write(friend.TeamId);
                    ack.Writer.Write(friend.TeamMarkId);
                    ack.Writer.Write(friend.State);

                    ack.Writer.Write(friend.Serial);
                    ack.Writer.Write(friend.LocationType);
                    ack.Writer.Write(friend.ChannelId);
                    ack.Writer.Write(friend.LocationId);
                    ack.Writer.Write(friend.Level);
                    ack.Writer.Write(friend.CurCarGrade);
                    ack.Writer.Write(friend.Serial);
                }
                client.Send(ack);
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
        public string CharacterName;
        public string TeamName;
        public long CharacterId;
        public long TeamId;
        public long TeamMarkId;
        public int State; // FriendState (0 = Blocked?)

        public uint Serial;
        public char LocationType; // byte..?
        public char ChannelId;
        public ushort LocationId;

        public ushort Level;
        public ushort CurCarGrade;
    }
}
