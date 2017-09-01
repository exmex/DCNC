using System;
using System.IO;
using System.Runtime.CompilerServices;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// TODO: Wrong Packet Size. CMD(86) CmdLen: : 56, AnalysisSize: 24
    /// TODO: Buy car still has not the correct structure.
    /// </summary>
    public class BuyCarAnswer : OutPacket
    {
        public int Unknown1;
        public short Unknown2;
        public int Price;
        public XiStrCarInfo CarInfo = new XiStrCarInfo();

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.BuyCarAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    CarInfo.Serialize(bs);
                    bs.Write(Unknown1); // ?????
                    bs.Write(Unknown2); // ?????
                    bs.Write(new byte[10]); // ?????
                    bs.Write(Price); // Price
                    //ack.Writer.Write(Bumper);
                }
#if DEBUG
                return ms.ToArray();
#else
                return new byte[0];
#endif
            }
        }
    }
}