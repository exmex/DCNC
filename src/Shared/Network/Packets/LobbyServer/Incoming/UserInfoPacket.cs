namespace Shared.Network.LobbyServer
{
    public class UserInfoPacket
    {
        /// <summary>
        /// The currently logged in username
        /// </summary>
        public string Username;

        /// <summary>
        /// The currently logged in user ticket
        /// </summary>
        public uint Ticket;

        public UserInfoPacket(Packet packet)
        {
            Username = packet.Reader.ReadUnicodeStatic(32);
            Ticket = packet.Reader.ReadUInt32();
        }
    }
}
