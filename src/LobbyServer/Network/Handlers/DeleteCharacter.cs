using Shared.Models;
using Shared.Network;
using Shared.Network.LobbyServer;

namespace LobbyServer.Network.Handlers
{
    public class DeleteCharacter
    {
        [Packet(Packets.CmdDeleteChar)]
        public static void Handle(Packet packet)
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