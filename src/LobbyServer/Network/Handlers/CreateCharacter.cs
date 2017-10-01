using Shared.Models;
using Shared.Network;
using Shared.Network.LobbyServer;
using Shared.Objects;
using Shared.Util;

namespace LobbyServer.Network.Handlers
{
    public static class CreateCharacter
    {
        [Packet(Packets.CmdCreateChar)]
        public static void Handle(Packet packet)
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
    }
}