using Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Network.Packets.GameServer.BattleZone;

namespace GameServer.Network.Handlers.BattleZone
{
    public class WMNicknameUpdate
    {
        [Packet(Packetss.CmdWMNickNameUpdate)]
        public static void Handle(Packet packet)
        {
            var wMNickNameUpdatePacket = new WMNickNameUpdatePacket(packet);
            var WMNickname = wMNickNameUpdatePacket.WMNickname;

            var ack = new Packet(Packetss.WMNickNameUpdateAck);

            ack.Writer.Write(WMNickname);

            packet.Sender.Send(ack);

        }


    }
}
