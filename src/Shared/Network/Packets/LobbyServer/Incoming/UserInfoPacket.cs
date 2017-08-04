namespace Shared.Network.LobbyServer
{
    public class UserInfoPacket
    {
        /// <summary>
        ///     The currently logged in user ticket reported by client
        ///     DO NOT TRUST! RE-CHECK WITH SERVER VALUES
        /// </summary>
        public readonly uint Ticket;

        /// <summary>
        ///     The currently logged in username reported by client
        ///     DO NOT TRUST! RE-CHECK WITH SERVER VALUES
        /// </summary>
        public readonly string Username;

        public UserInfoPacket(Packet packet)
        {
            Username = packet.Reader.ReadUnicodeStatic(32);
            Ticket = packet.Reader.ReadUInt32();
        }
    }
}