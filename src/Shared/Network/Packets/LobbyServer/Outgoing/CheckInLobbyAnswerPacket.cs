using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.LobbyServer
{
    public class CheckInLobbyAnswerPacket
    {
        public int Result = 1;

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
