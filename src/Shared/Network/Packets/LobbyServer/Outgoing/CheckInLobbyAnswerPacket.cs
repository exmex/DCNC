using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.LobbyServer
{
    public class CheckInLobbyAnswerPacket
    {
        /// <summary>
        /// The result of the operation
        /// </summary>
        public int Result = 1;

        /// <summary>
        /// The permission flags
        /// Valid values:
        /// 0x8000 => Administrator
        /// 0x4000 => Power User
        /// 0x2000 => Remote Client User
        /// 0x1000 => Developer
        /// 0x0 => User
        /// </summary>
        public int Permission = 0;

        /// <summary>
        /// Sends the answer packet.
        /// </summary>
        /// <param name="packetId">The packet identifier.</param>
        /// <param name="client">The client to send the packet to.</param>
        public void Send(ushort packetId, Client client)
        {
            var ack = new Packet(packetId);
            ack.Writer.Write(Result);
            ack.Writer.Write(Permission);
            client.Send(ack);
        }
    }
}