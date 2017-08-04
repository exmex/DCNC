namespace Shared.Network.LobbyServer
{
    public class CheckCharacterNamePacket
    {
        public readonly string CharacterName;

        public CheckCharacterNamePacket(Packet packet)
        {
            CharacterName = packet.Reader.ReadUnicode();
        }
    }
}