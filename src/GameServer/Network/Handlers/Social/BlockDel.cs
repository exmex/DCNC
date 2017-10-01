using Shared.Models;
using Shared.Network;

namespace GameServer.Network.Handlers.Social
{
    public class BlockDel
    {
        [Packet(Packets.CmdBlockDel)]
        public static void Handle(Packet packet)
        {
            var charName = packet.Reader.ReadUnicodeStatic(21);

            FriendModel.Delete(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId, charName);
        }
    }
}