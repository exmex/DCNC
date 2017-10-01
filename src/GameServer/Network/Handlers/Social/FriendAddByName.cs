using Shared.Models;
using Shared.Network;

namespace GameServer.Network.Handlers.Social
{
    public class FriendAddByName
    {
        [Packet(Packets.CmdFriendAddByName)]
        public static void Handle(Packet packet)
        {
            var charName = packet.Reader.ReadUnicodeStatic(21);

            //TODO: Send friend request instead of instantly adding him.
            if (FriendModel.AddByName(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId,
                charName))
                FriendList.Handle(packet);
        }
    }
}