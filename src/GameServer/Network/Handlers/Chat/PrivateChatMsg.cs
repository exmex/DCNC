using Shared.Network;

namespace GameServer.Network.Handlers
{
    public class PrivateChatMsg
    {
        [Packet(Packets.CmdPrivateChatMsg)]
        public static void Handle(Packet packet)
        {
            // TODO: It's somehow missing the user?!
            var message = packet.Reader.ReadUnicodePrefixed();
            packet.Sender.SendError("User not found.");
            
            //GameServer.Instance.Server.GetClient();
        }
    }
}