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
                    
                    // Client seems to ignore the next 34 bytes, but requires them to be sent.
                    /*
                        v3 = a3->field_0 == 1;
                        sub_442790();
                        sub_6F84E0(*(_DWORD *)(*(_DWORD *)(v4 + 344) + 688), a1, v3);
                        v5 = a3->field_0 == 1;
                        sub_442790();
                        sub_67B690(*(_DWORD *)(*(_DWORD *)(v6 + 344) + 496), a1, v5);
                    */
                    bs.Write(new byte[34]);
                    //bs.WriteUnicodeStatic(TeamName, 12);
                }
                return ms.ToArray();
            }
        }
    }
}