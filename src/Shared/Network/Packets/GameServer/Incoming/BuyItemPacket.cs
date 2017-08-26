namespace Shared.Network.GameServer
{
    public class BuyItemPacket
    {
        public int ItemId;
        public int Unknown;
        public int Quantity;

        public BuyItemPacket(Packet packet)
        {
            ItemId = packet.Reader.ReadInt16();
            Unknown = packet.Reader.ReadInt16();
            Quantity = packet.Reader.ReadInt16();
        }
    }
}