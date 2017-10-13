using System.IO;
using System.Numerics;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.AreaServer
{
    public class MoveVehicleAnswer : OutPacket
    {
        public ushort VehicleSerial;

        public byte[] Movement = new byte[106];
        /*public XiCarAttr CarAttr;
        public int GlobalTime;
        public Vector4 Position;
        public Vector4 Velocity;
        public ushort Progress;*/
        
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

        public override int ExpectedSize() => 110;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(VehicleSerial);
                    bs.Write(Movement);
                    /*bs.Write(new byte[24]); // CarAttr
                    bs.Write(GlobalTime);
                    bs.Write(Position);
                    bs.Write(Velocity);
                    bs.Write(Progress);*/
                }
                return ms.ToArray();
            }
        }
    }
}