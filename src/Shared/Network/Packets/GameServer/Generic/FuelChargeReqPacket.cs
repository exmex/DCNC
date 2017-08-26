namespace Shared.Network.GameServer
{
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