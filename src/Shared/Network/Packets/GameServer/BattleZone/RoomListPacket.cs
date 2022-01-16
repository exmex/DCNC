using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.Packets.GameServer.BattleZone
{
    public class RoomListPacket
    {
        public uint XiPvpRoomFilter; //m_RoomFilter;
        public uint m_Page;
        public uint m_PageSize;

        public RoomListPacket(Packet packet)
        {
            
            XiPvpRoomFilter = packet.Reader.ReadUInt32();
            m_Page = packet.Reader.ReadUInt32();
            m_PageSize = packet.Reader.ReadUInt32();

        }
        /*  XiPvpRoomFilter m_RoomFilter;
              unsigned int m_Page;
              unsigned int m_PageSize;*/
    }
}
