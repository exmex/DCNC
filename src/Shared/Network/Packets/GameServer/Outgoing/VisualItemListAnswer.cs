using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class VisualItemListAnswer : OutPacket
    {
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.VisualItemListAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(262144);
                    bs.Write(0);
                    bs.Write(new byte[120]);
                }
                #if !DEBUG
                throw new NotImplementedException();
                #endif
                return ms.GetBuffer();
                
            }
            /*
            var ack = new Packet(Packets.VisualItemListAck);
            ack.Writer.Write(262144); // ListUpdate (262144 = First packet from list queue, 262145 = sequential)
            ack.Writer.Write(0); // ItemNum
            ack.Writer.Write(new byte[120]); // Null VisualItem (120 bytes per XiStrMyVSItem)
            packet.Sender.Send(ack);
            */
        }
    }
}