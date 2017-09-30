using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_52EB00
    /// </summary>
    public class ItemModListAnswer : OutPacket
    {
        public ItemMod[] Items = new ItemMod[0];

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.ItemModListAck);
        }

        public override int ExpectedSize() => (100 * Items.Length - 1) + 106;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Items.Length);
                    foreach (var item in Items)
                    {
                        bs.Write(item);
                    }
                }
                return ms.ToArray();
            }
        }
    }
}