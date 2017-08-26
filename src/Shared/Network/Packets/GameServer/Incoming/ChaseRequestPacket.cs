namespace Shared.Network.GameServer
{
    public class ChaseRequestPacket
    {
        public readonly bool BNow;
        public readonly float PosX;
        public readonly float PosY;
        public readonly float PosZ;
        public readonly float Rot;

        public ChaseRequestPacket(Packet packet)
        {
            BNow = packet.Reader.ReadBoolean();
            PosX = packet.Reader.ReadSingle();
            PosY = packet.Reader.ReadSingle();
            PosZ = packet.Reader.ReadSingle();
            Rot = packet.Reader.ReadSingle();
        }
    }
}