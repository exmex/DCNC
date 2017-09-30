using System.IO;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_52751
    /// </summary>
    public class InstantGoalPlaceAnswer : OutPacket
    {
        /*
        struct XiStrExpInfo
        {
            unsigned int CurExp;
            unsigned int NextExp;
            unsigned int BaseExp;
        };
        __unaligned __declspec(align(1)) unsigned int TableIdx;
        __unaligned __declspec(align(1)) unsigned int PlaceIdx;
        __unaligned __declspec(align(1)) unsigned int GetExp;
        __unaligned __declspec(align(1)) XiStrExpInfo ExpInfo;
        unsigned __int16 Level;
        unsigned int InvenIdx;
        */
        public uint PlaceIndex;
        public uint TableIndex;
        public int EXP;
        public byte[] Unknown1 = new byte[28];

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.InstantGoalPlaceAck);
        }
        
        public override int ExpectedSize() => 44;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(TableIndex);
                    bs.Write(PlaceIndex);
                    bs.Write(EXP);
                    bs.Write(Unknown1);
                }
                return ms.ToArray();
            }
        }
    }
}