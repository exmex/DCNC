using System;
using System.IO;
using System.Runtime.CompilerServices;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_520FC0
    /// </summary>
    public class BuyCarAnswer : OutPacket
    {
        public int Price;
        public XiStrCarInfo CarInfo = new XiStrCarInfo();

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.BuyCarAck);
        }
        
        public override int ExpectedSize() => 56;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    CarInfo.Serialize(bs);
                    bs.Write(Price); // Price
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