namespace Shared.Network.GameServer
{
    /// <summary>
    /// 000000: 01 00 00 00 01 00 00 00 00 00 00 00 00 00 80 3F  · · · · · · · · · · · · · · · ?
    /// 000016: 00 00 00 00  · · · ·
    /// </summary>
    public class FuelChargeReqPacket
    {
        public readonly uint CarId;
        public readonly long Pay;
        public readonly float Fuel;
        
        public FuelChargeReqPacket(Packet packet)
        {
            CarId = packet.Reader.ReadUInt32();
            Pay = packet.Reader.ReadInt64();
            Fuel = packet.Reader.ReadSingle();
        }
    }
}