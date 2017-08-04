namespace Shared.Network.AreaServer
{
    public class TimeSyncPacket
    {
        public uint LocalTime;

        public TimeSyncPacket(Packet packet)
        {
            LocalTime = packet.Reader.ReadUInt32();
        }
    }
}