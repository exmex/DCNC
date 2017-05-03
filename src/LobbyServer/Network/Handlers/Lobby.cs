using System;
using System.Diagnostics;
using System.Text;
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
                Log.Error("Rejecting packet from {0}:{1} (user: {2} vs {3}, ticket {4} vs {5}) for invalid user-ticket combination.",
                    packet.Sender.EndPoint.Address.ToString(),
                    packet.Sender.EndPoint.Port,
                    userInfoPacket.Username,
                    packet.Sender.User.Name,
                    userInfoPacket.Ticket,
                    packet.Sender.User.Ticket);

#if DEBUG
                packet.Sender.Error("Invalid ticket-user combination.");
                packet.Sender.Kill();
#else
                packet.Sender.Kill();
#endif
            }

            new GameSettingsAnswerPacket().Send(Packets.GameSettingsAck, packet.Sender);

            packet.Sender.User.Characters = CharacterModel.Retrieve(LobbyServer.Instance.Database.Connection,
                packet.Sender.User.UID);

            var userInfoAnswerPacket = new UserInfoAnswerPacket
            {
                CharacterCount = packet.Sender.User.Characters.Count,
                Username = packet.Sender.User.Name,
                Characters = packet.Sender.User.Characters.ToArray()
            };

            userInfoAnswerPacket.Send(Packets.UserInfoAck, packet.Sender);
        }

        [Packet(Packets.CmdCheckInLobby)]
        public static void CheckInLobby(Packet packet)
        {
            var checkInLobbyPacket = new CheckInLobbyPacket(packet);
            if (checkInLobbyPacket.ProtocolVersion != Shared.ServerMain.ProtocolVersion)
            {
#if DEBUG
                packet.Sender.Error("Invalid protocol.");
#else
                packet.Sender.Kill("Too old client");
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
                packet.Sender.Error("Invalid ticket-user combination.");
#else
                checkInLobbyAnswerPacket.Send(Packets.CheckInLobbyAck, packet.Sender);
                packet.Sender.Kill("");
#endif
                return;
            }
            packet.Sender.User = user;

            // Send check in lobby answer.
            checkInLobbyAnswerPacket.Result = 0;
            checkInLobbyAnswerPacket.Permission = 0x8000; // TODO: Use account model instead.
            checkInLobbyAnswerPacket.Send(Packets.CheckInLobbyAck, packet.Sender);

            // Send current lobby time.
            var lobbyTimeAnswerPacket = new LobbyTimeAnswerPacket();
            lobbyTimeAnswerPacket.Send(Packets.LobbyTimeAck, packet.Sender);
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
            checkCharacterNameAnswerPacket.Send(Packets.CheckCharNameAck, packet.Sender);
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

            var createCharAnswerPacket = new CreateCharAnswerPacket
            {
                CharacterName = createCharPacket.CharacterName
            };
            createCharAnswerPacket.Send(Packets.CreateCharAck, packet.Sender);
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

                new DeleteCharacterAnswerPacket
                {
                    CharacterName = deleteCharacterPacket.CharacterName
                }.Send(Packets.DeleteCharAck, packet.Sender);

                return;
            }

#if DEBUG
            packet.Sender.Error("This character doesn't belong to you!");
#else
            packet.Sender.Kill("Suspected hack.");
#endif
        }
    }
}