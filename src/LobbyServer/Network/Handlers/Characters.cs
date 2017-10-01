using Shared.Models;
using Shared.Network;
using Shared.Network.LobbyServer;
using Shared.Objects;
using Shared.Util;

namespace LobbyServer.Network.Handlers
{
    public class Characters
    {
        [Packet(Packets.CmdCheckCharName)]
        public static void CheckCharacterName(Packet packet)
        {
            var checkCharacterNamePacket = new CheckCharacterNamePacket(packet);

            var nameTaken = CharacterModel.CheckNameExists(LobbyServer.Instance.Database.Connection,
                checkCharacterNamePacket.CharacterName);

            var checkCharacterNameAnswerPacket = new CheckCharacterNameAnswerPacket
            {
                CharacterName = checkCharacterNamePacket.CharacterName,
                Availability = !nameTaken,
            };
            packet.Sender.Send(checkCharacterNameAnswerPacket.CreatePacket());
        }

        [Packet(Packets.CmdCreateChar)]
        public static void CreateChar(Packet packet)
        {
            var createCharPacket = new CreateCharPacket(packet);

            if (packet.Sender.User == null)
            {
                packet.Sender.KillConnection("Session Invalid.");
                return;
            }
            
            var nameTaken = CharacterModel.CheckNameExists(LobbyServer.Instance.Database.Connection,
                createCharPacket.CharacterName);
            if (nameTaken)
            {
                packet.Sender.SendError("Character name taken!");
                return;
            }

            if (createCharPacket.CarType != 95 || createCharPacket.Color != 16777218)
            {
                Log.Error("Client {0} sent invalid car data!", packet.Sender.EndPoint.Address.ToString());
                packet.Sender.SendError("Invalid car.");
                return;
            }

            var character = new Character()
            {
                Uid = packet.Sender.User.Id,
                Name = createCharPacket.CharacterName,
                Avatar = createCharPacket.Avatar,
                MitoMoney = LobbyServer.Instance.Config.Lobby.NewCharacterMito,
                Hancoin = LobbyServer.Instance.Config.Lobby.NewCharacterHancoin
            };
            CharacterModel.CreateCharacter(LobbyServer.Instance.Database.Connection, ref character);
            
            character.ActiveVehicleId = (uint)VehicleModel.Create(LobbyServer.Instance.Database.Connection, new Vehicle()
            {
                CarType = createCharPacket.CarType,
                Color = createCharPacket.Color,
            }, character.Id);
            CharacterModel.Update(LobbyServer.Instance.Database.Connection, character);
            
            packet.Sender.Send(new CreateCharAnswerPacket
            {
                CharacterName = createCharPacket.CharacterName,
                CharacterId = character.Id,
                ActiveVehicleId = (int)character.ActiveVehicleId,
            }.CreatePacket());
        }

        [Packet(Packets.CmdDeleteChar)]
        public static void DeleteCharacter(Packet packet)
        {
            var deleteCharacterPacket = new DeleteCharacterPacket(packet);

            // Check if the user owns the character, if not don't do anything.
            var cid = CharacterModel.HasCharacter(LobbyServer.Instance.Database.Connection,
                deleteCharacterPacket.CharacterName,
                packet.Sender.User.Id);
            if (cid != 0)
            {
                CharacterModel.DeleteCharacter(LobbyServer.Instance.Database.Connection,
                    cid, packet.Sender.User.Id);

                packet.Sender.Send(new DeleteCharacterAnswerPacket
                {
                    CharacterName = deleteCharacterPacket.CharacterName
                }.CreatePacket());

                return;
            }

#if DEBUG
            packet.Sender.SendError("This character doesn't belong to you!");
#else
            packet.Sender.KillConnection("Tried to delete a character he doesn't own");
#endif
        }
    }
}