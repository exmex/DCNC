namespace Shared.Network.GameServer
{
    public class BuyVisualItemThreadPacket
    {
        public uint TableIndex;
        public uint CarId;
        public string PlateName;
        public uint PeriodIdx;

        public BuyVisualItemThreadPacket(Packet packet)
        {
            TableIndex = packet.Reader.ReadUInt32();
            CarId = packet.Reader.ReadUInt32();
            PlateName = packet.Reader.ReadUnicodeStatic(10);
            
            packet.Reader.ReadInt32(); // Unknown
            packet.Reader.ReadInt32(); // Unknown
            packet.Reader.ReadInt32(); // Unknown
            packet.Reader.ReadInt32(); // Unknown
            packet.Reader.ReadInt32(); // Unknown
            // 20 bytes

            PeriodIdx = packet.Reader.ReadUInt32();

            packet.Reader.ReadInt16(); // Unknown / 2 bytes
            packet.Reader.ReadInt64(); // curCash

            packet.Reader.ReadInt32(); // Unknown
        }
    }
}