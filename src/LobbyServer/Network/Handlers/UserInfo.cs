using Shared.Network;
using Shared.Network.LobbyServer;
using Shared.Util;

namespace LobbyServer.Network.Handlers
{
    public class UserInfo
    {
        [Packet(Packets.CmdUserInfo)]
        public static void Handle(Packet packet)
        {
            var userInfoPacket = new UserInfoPacket(packet);

            Log.Debug("UserInfo request. (Username: {0}, Ticket: {1})", userInfoPacket.Username, userInfoPacket.Ticket);

            if (userInfoPacket.Ticket != packet.Sender.User.Ticket ||
                userInfoPacket.Username != packet.Sender.User.Username)
            {
                Log.Error(
                    "Rejecting packet from {0}:{1} (user: {2} vs {3}, ticket {4} vs {5}) for invalid user-ticket combination.",
                    packet.Sender.EndPoint.Address.ToString(),
                    packet.Sender.EndPoint.Port,
                    userInfoPacket.Username,
                    packet.Sender.User.Username,
                    userInfoPacket.Ticket,
                    packet.Sender.User.Ticket);

#if DEBUG
                packet.Sender.SendError("Invalid ticket-user combination.");
#endif
                packet.Sender.KillConnection();
            }

            // Send gamesettings
            packet.Sender.Send(new GameSettingsAnswer().CreatePacket());

            packet.Sender.Send(new UserInfoAnswerPacket
            {
                CharacterCount = packet.Sender.User.Characters.Count,
                Username = packet.Sender.User.Username,
                Characters = packet.Sender.User.Characters.ToArray()
            }.CreatePacket());
        }
    }
}