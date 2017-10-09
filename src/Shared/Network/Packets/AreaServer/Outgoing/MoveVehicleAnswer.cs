using System.IO;
using System.Numerics;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.AreaServer
{
    public class MoveVehicleAnswer : OutPacket
    {
        public ushort VehicleSerial;
        public ushort Age;
        public XiCarAttr CarAttr;
        public int GlobalTime;
        public Vector4 Position;
        public Vector4 Velocity;
        public ushort Progress;
        
        /*
        unsigned __int16 m_Serial;
        unsigned __int16 m_Age;
        XiCarAttr m_CarAttr;
        int m_dwGlobalTime;
        XiVec4 m_Pos;
        XiVec4 m_Vel;
        unsigned __int16 m_Progress;
        */
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdMoveVehicle);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                }
                return ms.ToArray();
            }
        }
    }
}