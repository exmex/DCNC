using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class ItemModListAnswer : OutPacket
    {
        public ItemMod[] Items;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.ItemModListAck);
        }

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