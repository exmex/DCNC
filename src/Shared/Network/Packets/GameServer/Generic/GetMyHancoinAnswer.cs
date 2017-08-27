using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class GetMyHancoinAnswer : OutPacket
    {
        public int Hancoins;
        public int Mileage;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.GetMyHancoinAck);
        }

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