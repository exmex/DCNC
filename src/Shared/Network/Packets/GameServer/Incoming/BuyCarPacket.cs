namespace Shared.Network.GameServer
{
    public class BuyCarPacket
    {
        public readonly uint Color;
        public readonly uint Bumper;
        public readonly uint CarType;
        public readonly string CharacterName;

        public BuyCarPacket(Packet packet)
        {
            CharacterName = packet.Reader.ReadUnicodeStatic(21);
            CarType = packet.Reader.ReadUInt32();
            Bumper = packet.Reader.ReadUInt16();
            Color = packet.Reader.ReadUInt16();
        }
    }
}