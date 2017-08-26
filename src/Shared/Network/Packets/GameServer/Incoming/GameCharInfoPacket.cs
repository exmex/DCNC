namespace Shared.Network.GameServer
{
    public class GameCharInfoPacket
    {
        public readonly string CharacterName;
        
        public GameCharInfoPacket(Packet packet)
        {
            CharacterName = packet.Reader.ReadUnicodeStatic(21);
        }
    }
}