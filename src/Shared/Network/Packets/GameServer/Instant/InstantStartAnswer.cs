using System.IO;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_534100
    /// PKTSIZE: Wrong Packet Size. CMD(993) CmdLen: : 11, AnalysisSize: 8
    /// </summary>
    public class InstantStartAnswer : OutPacket
    {
        public uint TableIndex;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.InstantStartAck);
        }
        
        public override int ExpectedSize() => 11;

        /// <summary>
        /// 00000002 TableIdx        dd ?
        /// 00000006 field_2         dd ?
        /// 0000000A field_3         db ?
        /// </summary>
        /// <returns></returns>
        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(TableIndex);
                    // int
                    // char
                    bs.Write(new byte[3]); // Missing?
                }
                return ms.ToArray();
            }
        }
    }
}