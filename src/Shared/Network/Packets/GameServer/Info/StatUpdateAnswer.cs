using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class StatUpdateAnswer : OutPacket
    {
        public XiStrStatInfo StatisticInfo = new XiStrStatInfo();
        public XiStrEnchantBonus EnchantBonus = new XiStrEnchantBonus();
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.StatUpdateAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    StatisticInfo.Serialize(bs);
                    EnchantBonus.Serialize(bs);
                }
                return ms.ToArray();
            }
        }
    }
}