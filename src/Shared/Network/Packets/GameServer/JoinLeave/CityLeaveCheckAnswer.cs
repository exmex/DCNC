using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class CityLeaveCheckAnswer : OutPacket
    {
        public uint Result;
        /* 0-Moon Palace, 1=Koinonia, 2=Cras, 3=Oros, 4=Taipei, 5=NeoOros, else szPassword? */
        public int CityId;
        public string Post1;
        public string Post2;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CityLeaveCheckAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Result);
                    bs.Write(CityId);
                    bs.WriteAsciiStatic(Post1, 255);
                    bs.WriteAsciiStatic(Post2, 255);
                }
                return ms.GetBuffer();
            }
        }
    }
}