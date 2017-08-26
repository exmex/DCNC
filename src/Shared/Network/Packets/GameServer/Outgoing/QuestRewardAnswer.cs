using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class QuestRewardAnswer : OutPacket
    {
        public uint TableIndex;
        public uint GetExp;
        public uint GetMoney;
        public ulong CurrentExp;
        public ulong NextExp;
        public ulong BaseExp;
        public ushort Level;
        public ushort ItemNum;
        public uint RewardItem1;
        public uint RewardItem2;
        public uint RewardItem3;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.QuestRewardAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(TableIndex);
                    bs.Write(GetExp);
                    bs.Write(GetMoney);
                    bs.Write(CurrentExp);
                    bs.Write(NextExp);
                    bs.Write(BaseExp);
                    bs.Write(Level);
                    bs.Write(ItemNum);
                    bs.Write(RewardItem1);
                    bs.Write(RewardItem2);
                    bs.Write(RewardItem3);
                }
                return ms.GetBuffer();
            }
        }
    }
}