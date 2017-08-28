using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class CharUpdateAnswer : OutPacket
    {
        public Character character;
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CharUpdateAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    character.Serialize(bs);
                }
                return ms.ToArray();
            }
        }
    }
}