using System;
using System.IO;
using Shared.Network.AreaServer;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class MyTeamInfoAnswer : OutPacket
    {
        public uint Action;
        public ulong CharacterId;
        public int Rank;

        public Team Team;

        // It is not clear if this is the age.
        public ushort Age = 0;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.MyTeamInfoAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Action); // Action (1003, 1004, 1031, 1034)
                    /* After the action id, it seems the rest until byte 16 is ignored? */
                    bs.Write(CharacterId);
                    bs.Write(Rank);
                    /* After the action id, it seems the rest above is ignored*/
                    
                    // This must be 664 bytes long starting at byte 18
                    Team.Serialize(bs);

                    bs.Write(Age);
                }
                return ms.GetBuffer();
            }
        }
    }
}