namespace Shared.Network.LobbyServer
{
    public class CreateCharPacket
    {
        public readonly short Avatar;
        public readonly uint CarType;
        public readonly string CharacterName;
        public readonly uint Color;

        public CreateCharPacket(Packet packet)
        {
            CharacterName = packet.Reader.ReadUnicodeStatic(21);
            Avatar = packet.Reader.ReadInt16();
            CarType = packet.Reader.ReadUInt32();
            Color = packet.Reader.ReadUInt32();
        }
    }
}