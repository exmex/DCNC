using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// TODO: Wrong Packet Size. CMD(1401) CmdLen: : 14, AnalysisSize: 12
    /// </summary>
    public class GetMyHancoinAnswer : OutPacket
    {
        public int Hancoins;
        public int Mileage;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.GetMyHancoinAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Hancoins); // Hancoins?
                    bs.Write(Mileage); // Mileage?
                }
                return ms.ToArray();
            }
        }
    }
}