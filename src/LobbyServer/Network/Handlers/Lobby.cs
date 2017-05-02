using System;
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

            if (userInfoPacket.Ticket != packet.Sender.User.Ticket)
            {
                Log.Error("Rejecting packet from {0} (ticket {1}) for invalid user-ticket combination.",
                    userInfoPacket.Username,
                    userInfoPacket.Ticket);
                packet.Sender.Error("Invalid ticket-user combination.");
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

            var user = AccountModel.GetSession(LobbyServer.Instance.Database.Connection, checkInLobbyPacket.Username,
                checkInLobbyPacket.Ticket);
            if (user == null)
            {
                Log.Error("Rejecting {0} (ticket {1}) for invalid user-ticket combination.", checkInLobbyPacket.Username,
                    checkInLobbyPacket.Ticket);
                packet.Sender.Error("Invalid ticket-user combination.");
                return;
            }
            packet.Sender.User = user;

            var checkInLobbyAnswerPacket = new CheckInLobbyAnswerPacket
            {
                Result = 0
            };
            checkInLobbyAnswerPacket.Send(Packets.CheckInLobbyAck, packet.Sender);

            var lobbyTimeAnswerPacket = new LobbyTimeAnswerPacket();
            lobbyTimeAnswerPacket.Send(Packets.LobbyTimeAck, packet.Sender);

            Log.Debug("CheckInLobby {0} {1} {2} {3} {4}", checkInLobbyPacket.ProtocolVersion, checkInLobbyPacket.Ticket,
                checkInLobbyPacket.Username, checkInLobbyPacket.Time,
                BitConverter.ToString(Encoding.UTF8.GetBytes(checkInLobbyPacket.StringTicket)));
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

            Console.WriteLine(createCharPacket.CharacterName + " " + createCharPacket.Avatar + " " +
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

            packet.Sender.Error("This character doesn't belong to you!");
        }
    }
}