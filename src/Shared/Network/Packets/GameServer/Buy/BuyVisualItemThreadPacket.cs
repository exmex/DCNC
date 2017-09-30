namespace Shared.Network.GameServer
{
    public class BuyVisualItemThreadPacket
    {
        public uint TableIndex;
        public uint CarId;
        public string PlateName;
        
        /// <summary>
        /// Length of time to buy
        /// 1 = 7 Days
        /// 2 = 30 Days
        /// 3 = 90 Days
        /// 4 = 0 Days
        /// 5 = Infinite Days
        /// </summary>
        public uint PeriodIdx;
        public bool UseMileage;
        public long Cash;

        public BuyVisualItemThreadPacket(Packet packet)
        {
            TableIndex = packet.Reader.ReadUInt32();
            CarId = packet.Reader.ReadUInt32();
            PlateName = packet.Reader.ReadUnicodeStatic(20);
            
            PeriodIdx = packet.Reader.ReadUInt32();
            UseMileage = packet.Reader.ReadUInt16() > 0;
            Cash = packet.Reader.ReadInt64();
        }
    }
}