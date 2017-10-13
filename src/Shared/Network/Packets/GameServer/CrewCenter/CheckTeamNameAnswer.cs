using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_546560
    /// </summary>
    public class CheckTeamNameAnswer : OutPacket
    {
        // Availability. true = Available, false = Unavailable.
        public bool Availability;

        public string TeamName;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CheckTeamNameAck);
        }

        public override int ExpectedSize() => 40;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Availability ? 1 : 0);
                    bs.WriteUnicodeStatic(TeamName, 12);
                }
                return ms.ToArray();
            }
        }
    }
}