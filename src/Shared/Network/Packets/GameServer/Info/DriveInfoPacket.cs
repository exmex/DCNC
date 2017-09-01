namespace Shared.Network.GameServer
{
    public class DriveInfoPacket
    {
        public uint CarId;
        public int Time;
        public float TotalDistance;
        public float TotalFuel;
        public float StepDistance;
        public float StepFuel;

        public DriveInfoPacket(Packet packet)
        {
            CarId = packet.Reader.ReadUInt32();
            Time = packet.Reader.ReadInt32();
            TotalDistance = packet.Reader.ReadSingle();
            TotalFuel = packet.Reader.ReadSingle();
            StepDistance = packet.Reader.ReadSingle();
            StepFuel = packet.Reader.ReadSingle();
        }
    }
}