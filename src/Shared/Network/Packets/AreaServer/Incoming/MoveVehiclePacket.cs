using System.Numerics;
using Shared.Objects;

namespace Shared.Network.AreaServer
{
    public class MoveVehiclePacket
    {
        public ushort VehicleSerial;
        public ushort Age;
        public int GlobalTime;
        public Vector4 Position;
        public Vector4 Velocity;
        public ushort Progress;

        public MoveVehiclePacket(Packet packet)
        {
            VehicleSerial = packet.Reader.ReadUInt16();
            Age = packet.Reader.ReadUInt16();
            packet.Reader.ReadUInt16(); // Sort
            packet.Reader.ReadUInt16(); // Body
            packet.Reader.ReadUInt16(); // Color1
            packet.Reader.ReadUInt16(); // Color2
            packet.Reader.ReadUInt16(); // Color3
            packet.Reader.ReadUInt16(); // Color4
            packet.Reader.ReadInt32(); // lvalSortBody
            packet.Reader.ReadInt32(); // lvalSortColor
            packet.Reader.ReadInt64(); // llval?
            GlobalTime = packet.Reader.ReadInt32();
            Position = packet.Reader.ReadVector4();
            Velocity = packet.Reader.ReadVector4();
            Progress = packet.Reader.ReadUInt16();
            
            // char m_data[44]; <-- THE FUCK!?
        }
    }
}