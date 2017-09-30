using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_52A8B0
    /// </summary>
    public class GetMyHancoinAnswer : OutPacket
    {
        public int Hancoins;
        public long Mileage;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.GetMyHancoinAck);
        }
        
        public override int ExpectedSize() => 14;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Hancoins); // Hancoins?
                    bs.Write(Mileage); // Mileage?
                }
                return ms.ToArray();
            }
        }
    }
}