using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Shared.Util;
using Shared.Network.Packets.GameServer;

namespace Shared.Network.Packets.GameServer.BattleZone
{
    public class RoomListAnswer : OutPacket
    {
        public uint XiPvpRoomFilter; //m_RoomFilter;
        public uint m_Page;
        public uint m_PageSize;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packetss.CmdRoomList);
        }

        public override int ExpectedSize() => 10;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    
                    bs.Write(XiPvpRoomFilter);
                    //bs.Write(m_RoomFilter);
                    bs.Write(m_Page);
                    bs.Write(m_PageSize);
                    
                }
                return ms.ToArray();
            }
        }

    }
}
