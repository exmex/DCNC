using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class CheckInGameAnswer : OutPacket
    {
        public uint Result;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CheckInGameAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Result);
                }
                return ms.GetBuffer();
            }
        }
    }
}