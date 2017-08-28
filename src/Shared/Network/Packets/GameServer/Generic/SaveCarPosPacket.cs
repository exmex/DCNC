namespace Shared.Network.GameServer
{
    /// <summary>
    /// 000000: 00 00 00 00 00 00 00 00 00 00 00 00 4A 70 AB 42  · · · · · · · · · · · · J p · B
    /// 000016: 00 00 00 00 01 00 00 00 01 00 00 00  · · · · · · · · · · · ·
    /// </summary>
    public class SaveCarPosPacket
    {
        public readonly int ChannelId;
        public readonly float X;
        public readonly float Y;
        public readonly float Z;
        public readonly float W;
        public readonly int CityId;
        public readonly int PosState;
        
        public SaveCarPosPacket(Packet packet)
        {
            ChannelId = packet.Reader.ReadInt32();
            X = packet.Reader.ReadSingle();
            Y = packet.Reader.ReadSingle();
            Z = packet.Reader.ReadSingle();
            W = packet.Reader.ReadSingle();
            CityId = packet.Reader.ReadInt32();
            PosState = packet.Reader.ReadInt32();
        }
    }
}