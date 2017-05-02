using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.LobbyServer
{
    public class LobbyTimeAnswerPacket
    {
        public int LocalTime;
        public int TimeT;

        public LobbyTimeAnswerPacket()
        {
            LocalTime = Environment.TickCount;
            TimeT = Environment.TickCount;
        }

        /// <summary>
        /// Sends the answer packet.
        /// </summary>
        /// <param name="packetId">The packet identifier.</param>
        /// <param name="client">The client to send the packet to.</param>
        public void Send(ushort packetId, Client client)
        {
            Packet packet = new Packet(packetId);
            packet.Writer.Write(LocalTime);
            packet.Writer.Write(TimeT);
            client.Send(packet);
        }
    }
}
