namespace Shared.Network.LobbyServer
{
    public class CreateCharAnswerPacket
    {
        public string CharacterName;

        /// <summary>
        ///     Sends the answer packet.
        /// </summary>
        /// <param name="packetId">The packet identifier.</param>
        /// <param name="client">The client to send the packet to.</param>
        public void Send(ushort packetId, Client client)
        {
            var ack = new Packet(packetId);
            ack.Writer.WriteUnicodeStatic(CharacterName, 21);
            client.Send(ack);
        }
    }
}