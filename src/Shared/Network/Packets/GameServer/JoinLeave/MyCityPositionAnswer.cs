using System.IO;
using System.Numerics;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_529600
    /// </summary>
    public class MyCityPositionAnswer : OutPacket
    {
        public int City;
        public int LastChannel;
        public Vector4 Position;
        public int PositionState;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.MyCityPositionAck);
        }
        
        public override int ExpectedSize() => 31;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(City);
                    bs.Write(LastChannel);
                    bs.Write(Position);
                    bs.Write(PositionState);
                    // Rice sends an extra byte for some reason??
                    //ack.Writer.Write((byte)0);
                }
                return ms.ToArray();
            }
        }
    }
}