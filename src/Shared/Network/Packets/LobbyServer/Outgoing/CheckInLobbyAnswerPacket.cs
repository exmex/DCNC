namespace Shared.Network.LobbyServer
{
    public class CheckInLobbyAnswerPacket
    {
        /// <summary>
        ///     The permission flags
        ///     Valid values:
        ///     0x8000 => Administrator
        ///     0x4000 => Power User
        ///     0x2000 => Remote Client User
        ///     0x1000 => Developer
        ///     0x0 => User
        /// </summary>
        public int Permission = 0;

        /// <summary>
        ///     The result of the operation
        /// </summary>
        public int Result = 1;

        /// <summary>
        ///     Sends the answer packet.
        /// </summary>
        /// <param name="client">The client to send the packet to.</param>
        public void Send(Client client)
        {
            var ack = new Packet(Packets.CheckInLobbyAck);
            ack.Writer.Write(Result);
            ack.Writer.Write(Permission);
            client.Send(ack);
        }
    }
}