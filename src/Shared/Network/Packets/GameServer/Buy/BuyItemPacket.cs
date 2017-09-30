namespace Shared.Network.GameServer
{
    public class BuyItemPacket
    {
        public int TableIndex;
        public uint Unknown;
        public uint Quantity;

        public BuyItemPacket(Packet packet)
        {
            TableIndex = packet.Reader.ReadInt32();
            Quantity = packet.Reader.ReadUInt32();
            Unknown = packet.Reader.ReadUInt32();
        }
    }
}