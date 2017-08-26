namespace Shared.Network.AuthServer
{
    public class GetDateTimePacket
    {
        public readonly uint GlobalTime;
        public readonly uint LocalTime;
        public readonly uint Action;
        
        public GetDateTimePacket(Packet packet)
        {
            GlobalTime = packet.Reader.ReadUInt32(); // GlobalTime
            LocalTime = packet.Reader.ReadUInt32(); // LocalTime
            Action = packet.Reader.ReadUInt32(); // Action
        }
    }
}