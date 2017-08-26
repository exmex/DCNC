using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class GetMyHancoinAnswer : IOutPacket
    {
        public int Hancoins;
        public int Mileage;
        
        public Packet CreatePacket()
        {
            var ack = new Packet(Packets.GetMyHancoinAck);
            ack.Writer.Write(GetBytes());
            return ack;
        }

        public byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Hancoins); // Hancoins?
                    bs.Write(Mileage); // Mileage?
                }
                return ms.GetBuffer();
            }
        }
    }
}