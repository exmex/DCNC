using System;
using System.Text;
using Shared;
using Shared.Models;
using Shared.Network;
using Shared.Network.LobbyServer;
using Shared.Objects;
using Shared.Util;

namespace LobbyServer.Network.Handlers
{
    public class Lobby
    {
        [Packet(Packets.CmdUserInfo)]
        public static void UserInfo(Packet packet)
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

        [Packet(Packets.CmdCheckInLobby)]
        public static void CheckInLobby(Packet packet)
        {
            var checkInLobbyPacket = new CheckInLobbyPacket(packet);
            if (checkInLobbyPacket.ProtocolVersion != ServerMain.ProtocolVersion)
            {
                packet.Sender.SendDebugError("Invalid protocol.");
#if !DEBUG
                packet.Sender.KillConnection("Client outdated!");
#endif
                return;
            }

            var checkInLobbyAnswerPacket = new CheckInLobbyAnswerPacket
            {
                Result = 1,
                Permission = 0x0
            };

            var user = AccountModel.RetrieveFromSession(LobbyServer.Instance.Database.Connection, checkInLobbyPacket.Username,
                checkInLobbyPacket.Ticket);

            Log.Debug("CheckInLobby {0} {1} {2} {3} {4}", checkInLobbyPacket.ProtocolVersion, checkInLobbyPacket.Ticket,
                checkInLobbyPacket.Username, checkInLobbyPacket.Time,
                BitConverter.ToString(Encoding.UTF8.GetBytes(checkInLobbyPacket.StringTicket)));

            // Check if session is really valid, and the client is not tricking us somehow.
            if (user == null)
            {
                Log.Error("Rejecting {0}:{1} (user {2} vs {3}, ticket {4} vs {5}) for invalid user-ticket combination.",
                    packet.Sender.EndPoint.Address.ToString(),
                    packet.Sender.EndPoint.Port,
                    checkInLobbyPacket.Username,
                    packet.Sender.User.Username,
                    checkInLobbyPacket.Ticket,
                    packet.Sender.User.Ticket);
#if DEBUG
                packet.Sender.SendError("Invalid ticket-user combination.");
#else
                packet.Sender.Send(checkInLobbyAnswerPacket.CreatePacket());
                packet.Sender.KillConnection("Invalid ticket-user combination.");
#endif
                return;
            }
            packet.Sender.User = user;
            packet.Sender.User.Characters = AccountModel.RetrieveCharacters(LobbyServer.Instance.Database.Connection, user.Id);

            // Send check in lobby answer.
            checkInLobbyAnswerPacket.Result = 0;
            checkInLobbyAnswerPacket.Permission = user.Permission;
            packet.Sender.Send(checkInLobbyAnswerPacket.CreatePacket());

            // Send current lobby time.
            packet.Sender.Send(new LobbyTimeAnswerPacket().CreatePacket());
        }
    }
}