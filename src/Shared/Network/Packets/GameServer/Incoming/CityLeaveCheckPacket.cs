namespace Shared.Network.GameServer
{
    public class CityLeaveCheckPacket
    {
        public readonly int CityId;
        public readonly string Post1;
        public readonly string Post2;

        public CityLeaveCheckPacket(Packet packet)
        {
            CityId = packet.Reader.ReadInt32();
            Post1 = packet.Reader.ReadAsciiStatic(255);
            Post2 = packet.Reader.ReadAsciiStatic(255);
        }
    }
}