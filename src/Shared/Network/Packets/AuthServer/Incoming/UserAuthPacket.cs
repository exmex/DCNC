namespace Shared.Network.AuthServer
{
    public class UserAuthPacket
    {
        /// <summary>
        ///     The password
        /// </summary>
        /// <remarks>STRING</remarks>
        public readonly string Password;

        /// <summary>
        ///     The protocol version
        /// </summary>
        /// <remarks>INT 4-Bytes</remarks>
        public readonly int ProtocolVersion;

        /// <summary>
        ///     The username
        /// </summary>
        /// <remarks>CHAR 80-Bytes</remarks>
        public readonly string Username;

        public UserAuthPacket(Packet packet)
        {
            ProtocolVersion = packet.Reader.ReadInt32();

            Username = packet.Reader.ReadUnicodeStatic(40);

            Password = packet.Reader.ReadAscii();
            Password = Password.Substring(0, Password.Length - 1);

            // TODO: Missing 62 bytes of data informations
        }
    }
}