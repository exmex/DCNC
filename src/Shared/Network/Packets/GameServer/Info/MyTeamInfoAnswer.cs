using System;
using System.IO;
using Shared.Network.AreaServer;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_5462E0
    /// PKTSIZE: Wrong Packet Size. CMD(841) CmdLen: : 692, AnalysisSize: 393
    /// </summary>
    public class MyTeamInfoAnswer : OutPacket
    {
        public uint Action;
        public ulong CharacterId;
        public int Rank;

        public Crew Crew;

        // It is not clear if this is the age.
        public ushort Age = 0;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.MyTeamInfoAck);
        }
        
        public override int ExpectedSize() => 692;
        
        /*
        Client expects:
        
        field_2 => 0x298u
        field_6 => 0x1Cu
        
        00000002 action          dd ?
        00000006 charId          db 8 dup(?)
        0000000E rank            dd ?
        00000012 teamInfo        db 664 dup(?)
        000002AA field_3         dw ?
        000002AC field_4         dd ?
        000002B0 field_5         dd ?
        */

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    // MyTeamInfoAck always Action = lpbuffer+2
                    bs.Write(Action); // Action (1003, 1004, 1031, 1034)
                    bs.Write(CharacterId);
                    bs.Write(Rank);

                    if (Crew == null)
                        bs.Write(new Crew());
                    else
                        Crew.Serialize(bs);

                    bs.Write(Age);
                    bs.Write(0); // field_4
                    bs.Write(0); // field_5
                }
                return ms.ToArray();
            }
        }
    }
}