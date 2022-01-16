using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.Packets.GameServer.BattleZone
{
    public class GetRoomInfoPacket
    {
        public int m_Act;
        public uint m_RoomId;

        public GetRoomInfoPacket(Packet packet)
        {

            m_Act = packet.Reader.ReadInt32();
            m_RoomId = packet.Reader.ReadUInt32();
            
        }

    }
}
