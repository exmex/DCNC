using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_5282A0
    /// </summary>
    public class CharUpdateAnswer : OutPacket
    {
        public Character Character = new Character();
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CharUpdateAck);
        }
        
        public override int ExpectedSize() => 323;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    Character.Serialize(bs);
                }
                return ms.ToArray();
            }
        }
    }
}