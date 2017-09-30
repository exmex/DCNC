using System.Numerics;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// 000000: 00 00 00 00 00 00 00 00 00 00 00 00 4A 70 AB 42  · · · · · · · · · · · · J p · B
    /// 000016: 00 00 00 00 01 00 00 00 01 00 00 00  · · · · · · · · · · · ·
    /// </summary>
    public class SaveCarPosPacket
    {
        public readonly int ChannelId;
        public readonly Vector4 Position;
        public readonly int CityId;
        public readonly int PosState;
        
        public SaveCarPosPacket(Packet packet)
        {
            ChannelId = packet.Reader.ReadInt32();
            Position = packet.Reader.ReadVector4();
            CityId = packet.Reader.ReadInt32();
            PosState = packet.Reader.ReadInt32();
        }
    }
}