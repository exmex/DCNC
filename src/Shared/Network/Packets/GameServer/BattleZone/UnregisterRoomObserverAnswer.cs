using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Shared.Util;
using Shared.Objects;

namespace Shared.Network.Packets.GameServer.BattleZone
{
    public class UnregisterRoomObserverAnswer : OutPacket
    {
        

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packetss.CmdItemExpireCmd);
        }

        public override int ExpectedSize() => 10;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {

                    

                }
                return ms.ToArray();
            }
        }
    }
}
