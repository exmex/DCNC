using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Network.Packets.GameServer.BattleZone;
using Shared.Network;

namespace GameServer.Network.Handlers.BattleZone
{
    public class RoomList
    {
        [Packet(Packetss.CmdRoomList)]
        public static void Handle(Packet packet)
        {
            var RoomListPacket = new RoomListPacket(packet);
            packet.Sender.Send(new RoomListAnswer()
            {

                XiPvpRoomFilter = 4,
                //m_RoomFilter = 1,
                m_Page = 1,
                m_PageSize = 1,

            }.CreatePacket());
            /*  XiPvpRoomFilter m_RoomFilter;
              unsigned int m_Page;
              unsigned int m_PageSize;*/
        }
    }
}
