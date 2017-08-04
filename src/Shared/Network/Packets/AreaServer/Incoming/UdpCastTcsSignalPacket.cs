namespace Shared.Network.AreaServer
{
    public class UdpCastTcsSignalPacket
    {
        public readonly int AreaId;
        public readonly int Signal;
        public readonly int State;
        public readonly int Time;
        public readonly float X;
        public readonly float Y;

        public UdpCastTcsSignalPacket(Packet packet)
        {
            AreaId = packet.Reader.ReadInt32(); // AreaId
            X = packet.Reader.ReadSingle(); // X
            Y = packet.Reader.ReadSingle(); // Y

            Time = packet.Reader.ReadInt32(); // Time
            Signal = packet.Reader.ReadInt32(); // Signal
            State = packet.Reader.ReadInt32(); // State
        }
    }
}