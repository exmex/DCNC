namespace Shared.Network.GameServer
{
    public class LoadCharThreadPacket
    {
        public string CharacterName;
        public uint Serial;

        public LoadCharThreadPacket(Packet packet)
        {
            CharacterName = packet.Reader.ReadUnicode();
            Serial = packet.Reader.ReadUInt32();
        }
    }
}