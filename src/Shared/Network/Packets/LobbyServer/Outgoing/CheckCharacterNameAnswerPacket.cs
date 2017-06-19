using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.LobbyServer
{
    public class CheckCharacterNameAnswerPacket
    {
        public string CharacterName;

        // Availability. true = Available, false = Unavailable.
        public bool Availability;

        /// <summary>
        /// Sends the answer packet.
        /// </summary>
        /// <param name="client">The client to send the packet to.</param>
        public void Send(Client client)
        {
            var ack = new Packet(Packets.CheckCharNameAck);
            ack.Writer.WriteUnicodeStatic(CharacterName, 21);
            ack.Writer.Write(Availability);
            client.Send(ack);
        }
    }
}
