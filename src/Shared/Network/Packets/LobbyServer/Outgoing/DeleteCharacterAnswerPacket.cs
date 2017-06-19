using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.LobbyServer
{
    public class DeleteCharacterAnswerPacket
    {
        public string CharacterName;

        /// <summary>
        /// Sends the answer packet.
        /// </summary>
        /// <param name="client">The client to send the packet to.</param>
        public void Send(Client client)
        {
            var ack = new Packet(Packets.DeleteCharAck);
            ack.Writer.WriteUnicodeStatic(CharacterName, 21);
            client.Send(ack);
        }
    }
}
