namespace Shared.Network.GameServer
{
    public class SellItemPacket
    {
        public uint TableIndex;
        public uint Quantity;
        public uint Slot;

        public SellItemPacket(Packet packet)
        {
            TableIndex = packet.Reader.ReadUInt32();
            Quantity = packet.Reader.ReadUInt32();
            Slot = packet.Reader.ReadUInt32();
        }
    }
}