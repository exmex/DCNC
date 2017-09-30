using System.IO;
using System.Numerics;
using System.Threading;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_56B00
    /// </summary>
    public class ChaseRequestAnswer : OutPacket
    {
        public ushort VehicleSerial;
        public Vector4 StartPos;
        public Vector4 EndPos;

        public int CourseId;
        public int Type;
        public string PosName;
        public int FirstHuvLevel;
        public int FirstHuvId = 10001;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.ChasePropose);
        }
        
        public override int ExpectedSize() => 252;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(VehicleSerial); // Serial?
                    bs.Write(StartPos);
                    
                    bs.Write(EndPos);

                    bs.Write(Type); // Type?
                    bs.Write(CourseId); // CourseId

                    bs.Write(FirstHuvLevel); // HUV first level
                    bs.Write(FirstHuvId); // HUV first Id
                    bs.WriteUnicodeStatic(PosName, 100);
                }
                return ms.ToArray();
            }
        }
    }
}