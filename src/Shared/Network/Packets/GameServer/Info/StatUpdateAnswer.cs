using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_521CC0
    /// PKTSIZE: Wrong Packet Size. CMD(760) CmdLen: : 158, AnalysisSize: 152
    /// </summary>
    public class StatUpdateAnswer : OutPacket
    {
        public XiStrStatInfo StatisticInfo = new XiStrStatInfo();
        public XiStrEnchantBonus EnchantBonus = new XiStrEnchantBonus();
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.StatUpdateAck);
        }
        
        public override int ExpectedSize() => 158;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    for (var i = 0; i < 16; ++i)
                        bs.Write(0);
                    bs.Write(1000);
                    bs.Write(9001);
                    bs.Write(9002);
                    bs.Write(9003);
                    bs.Write(new byte[76]);
                    //StatisticInfo.Serialize(bs);
                    //EnchantBonus.Serialize(bs);
                }
                return ms.ToArray();
            }
            /* Pulled from Rice:
            var stat = new RicePacket(760); // StatUpdate
            for (int i = 0; i < 16; ++i)
                stat.Writer.Write(0);
            stat.Writer.Write(1000);
            stat.Writer.Write(9001);
            stat.Writer.Write(9002);
            stat.Writer.Write(9003);
            stat.Writer.Write(new byte[76]);
            packet.Sender.Send(stat);
            */
        }
    }
}