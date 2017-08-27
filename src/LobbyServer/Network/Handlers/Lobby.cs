using System;
using System.Text;
using Shared;
using Shared.Models;
using Shared.Network;
using Shared.Network.LobbyServer;
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
                userInfoPacket.Username != packet.Sender.User.Name)
            {
                Log.Error(
                    "Rejecting packet from {0}:{1} (user: {2} vs {3}, ticket {4} vs {5}) for invalid user-ticket combination.",
                    packet.Sender.EndPoint.Address.ToString(),
                    packet.Sender.EndPoint.Port,
                    userInfoPacket.Username,
                    packet.Sender.User.Name,
                    userInfoPacket.Ticket,
                    packet.Sender.User.Ticket);

#if DEBUG
                packet.Sender.SendError("Invalid ticket-user combination.");
#endif
                packet.Sender.KillConnection();
            }

            packet.Sender.Send(new GameSettingsAnswer().CreatePacket());
            
            packet.Sender.User.Characters = CharacterModel.Retrieve(LobbyServer.Instance.Database.Connection,
                packet.Sender.User.UID);

            packet.Sender.Send(new UserInfoAnswerPacket
            {
                CharacterCount = packet.Sender.User.Characters.Count,
                Username = packet.Sender.User.Name,
                Characters = packet.Sender.User.Characters.ToArray()
            }.CreatePacket());
        }

        [Packet(Packets.CmdCheckInLobby)]
        public static void CheckInLobby(Packet packet)
        {
            var checkInLobbyPacket = new CheckInLobbyPacket(packet);
            if (checkInLobbyPacket.ProtocolVersion != ServerMain.ProtocolVersion)
            {
#if DEBUG
                packet.Sender.SendError("Invalid protocol.");
#else
                packet.Sender.KillConnection("Client outdated!");
#endif
                return;
            }

            var checkInLobbyAnswerPacket = new CheckInLobbyAnswerPacket
            {
                Result = 1,
                Permission = 0x0
            };

            var user = AccountModel.GetSession(LobbyServer.Instance.Database.Connection, checkInLobbyPacket.Username,
                checkInLobbyPacket.Ticket);

            Log.Debug("CheckInLobby {0} {1} {2} {3} {4}", checkInLobbyPacket.ProtocolVersion, checkInLobbyPacket.Ticket,
                checkInLobbyPacket.Username, checkInLobbyPacket.Time,
                BitConverter.ToString(Encoding.UTF8.GetBytes(checkInLobbyPacket.StringTicket)));

            // Check is session is really valid, and the client is not tricking us somehow.
            if (user == null)
            {
                Log.Error("Rejecting {0}:{1} (user {2} vs {3}, ticket {4} vs {5}) for invalid user-ticket combination.",
                    packet.Sender.EndPoint.Address.ToString(),
                    packet.Sender.EndPoint.Port,
                    checkInLobbyPacket.Username,
                    packet.Sender.User.Name,
                    checkInLobbyPacket.Ticket,
                    packet.Sender.User.Ticket);
#if DEBUG
                packet.Sender.SendError("Invalid ticket-user combination.");
#else
                packet.Sender.Send(checkInLobbyAnswerPacket.CreatePacket());
                packet.Sender.KillConnection("");
#endif
                return;
            }
            packet.Sender.User = user;

            // Send check in lobby answer.
            checkInLobbyAnswerPacket.Result = 0;
            checkInLobbyAnswerPacket.Permission = 0x8000; // TODO: Use account model instead.
            packet.Sender.Send(checkInLobbyAnswerPacket.CreatePacket());

            // Send current lobby time.
            packet.Sender.Send(new LobbyTimeAnswerPacket().CreatePacket());
        }

        [Packet(Packets.CmdCheckCharName)]
        public static void CheckCharacterName(Packet packet)
        {
            var checkCharacterNamePacket = new CheckCharacterNamePacket(packet);

            var checkCharacterNameAnswerPacket = new CheckCharacterNameAnswerPacket
            {
                CharacterName = checkCharacterNamePacket.CharacterName,
                Availability =
                    !CharacterModel.Exists(LobbyServer.Instance.Database.Connection,
                        checkCharacterNamePacket.CharacterName)
            };
            packet.Sender.Send(checkCharacterNameAnswerPacket.CreatePacket());
        }

        [Packet(Packets.CmdCreateChar)]
        public static void CreateChar(Packet packet)
        {
            var createCharPacket = new CreateCharPacket(packet);

            // TODO: Check if the values send by client are valid.

            Log.Debug(createCharPacket.CharacterName + " " + createCharPacket.Avatar + " " +
                      createCharPacket.CarType + " " + createCharPacket.Color);

            CharacterModel.CreateCharacter(LobbyServer.Instance.Database.Connection, packet.Sender.User.UID,
                createCharPacket.CharacterName, createCharPacket.Avatar, createCharPacket.CarType,
                createCharPacket.Color);

            packet.Sender.Send(new CreateCharAnswerPacket
            {
                CharacterName = createCharPacket.CharacterName
            }.CreatePacket());
        }

        [Packet(Packets.CmdDeleteChar)]
        public static void DeleteCharacter(Packet packet)
        {
            var deleteCharacterPacket = new DeleteCharacterPacket(packet);

            // Check if the user owns the character, if not don't do anything.
            if (CharacterModel.HasCharacter(LobbyServer.Instance.Database.Connection, deleteCharacterPacket.CharacterId,
                packet.Sender.User.UID))
            {
                CharacterModel.DeleteCharacter(LobbyServer.Instance.Database.Connection,
                    deleteCharacterPacket.CharacterId, packet.Sender.User.UID);

                packet.Sender.Send(new DeleteCharacterAnswerPacket
                {
                    CharacterName = deleteCharacterPacket.CharacterName
                }.CreatePacket());

                return;
            }

#if DEBUG
            packet.Sender.SendError("This character doesn't belong to you!");
#else
            packet.Sender.KillConnection("Suspected hack.");
#endif
        }
    }
}