namespace Shared.Network.LobbyServer
{
    public class CheckCharacterNameAnswerPacket
    {
        // Availability. true = Available, false = Unavailable.
        public bool Availability;

        public string CharacterName;

        /// <summary>
        ///     Sends the answer packet.
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