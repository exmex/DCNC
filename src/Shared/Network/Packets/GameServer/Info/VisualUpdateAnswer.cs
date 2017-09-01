using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class VisualUpdateAnswer : OutPacket
    {
        public ushort Serial;
        public ushort Age;
        public uint CarId;
        public XiStrCarInfo CarInfo = new XiStrCarInfo();
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.VisualUpdateAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Serial);
                    bs.Write(Age);
                    bs.Write(CarId);
                    
                    CarInfo.Serialize(bs);
                }
                return ms.ToArray();
            }
        }
    }
}