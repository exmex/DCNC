namespace Shared.Network.GameServer
{
    public class BuyItemPacket
    {
        public int TableIndex;
        public int Unknown;
        public int Quantity;

        public BuyItemPacket(Packet packet)
        {
            TableIndex = packet.Reader.ReadInt32();
            Quantity = packet.Reader.ReadInt32();
            Unknown = packet.Reader.ReadInt32();
        }
    }
}