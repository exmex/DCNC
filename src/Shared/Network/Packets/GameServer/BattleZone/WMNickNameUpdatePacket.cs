using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.Packets.GameServer.BattleZone
{
    public  class WMNickNameUpdatePacket
    {
        public readonly string WMNickname;

        public WMNickNameUpdatePacket(Packet packet)
        {

            WMNickname = packet.Reader.ReadString(); // World Match Nickname
        }


    }
}
