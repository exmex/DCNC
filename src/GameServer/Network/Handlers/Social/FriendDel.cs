using Shared.Models;
using Shared.Network;

namespace GameServer.Network.Handlers.Social
{
    public class FriendDel
    {
        [Packet(Packets.CmdFriendDel)]
        public static void Handle(Packet packet)
        {
            /*
            [Debug] - FriendDel: 000000: 54 00 45 00 53 00 54 00 49 00 4E 00 47 00 00 00  T · E · S · T · I · N · G · · ·
            000016: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · ·
            000032: 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · ·

            [Info] - Received FriendDel (id 235, 0xEB) on 11021. 
            */
            var charName = packet.Reader.ReadUnicodeStatic(21);

            FriendModel.Delete(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId, charName);

            FriendList.Handle(packet);
        }
    }
}