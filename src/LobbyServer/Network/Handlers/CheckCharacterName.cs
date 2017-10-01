using Shared.Models;
using Shared.Network;
using Shared.Network.LobbyServer;

namespace LobbyServer.Network.Handlers
{
    public static class CheckCharacterName
    {
        [Packet(Packets.CmdCheckCharName)]
        public static void Handle(Packet packet)
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
    }
}