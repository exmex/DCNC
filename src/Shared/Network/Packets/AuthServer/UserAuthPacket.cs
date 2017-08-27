using MySql.Data.MySqlClient;
using Shared.Models;
using Shared.Util;

namespace Shared.Network.AuthServer
{
    public class UserAuthPacket
    {
        /// <summary>
        ///     The password
        /// </summary>
        /// <remarks>char 64-Bytes</remarks>
        public readonly string Password;

        /// <summary>
        ///     The protocol version
        /// </summary>
        /// <remarks>int 4-Bytes</remarks>
        public readonly uint ProtocolVersion;

        /// <summary>
        ///     The username
        /// </summary>
        /// <remarks>wchar_t 40-Bytes + null-terminators 40-Bytes</remarks>
        public readonly string Username;

        /// <summary>
        ///     Constructs and reads the UserAuth packet
        /// </summary>
        /// <param name="packet">The packet that got received</param>
        public UserAuthPacket(Packet packet)
        {
            ProtocolVersion = packet.Reader.ReadUInt32();

            Username = packet.Reader.ReadUnicodeStatic(40);
            
            Password = packet.Reader.ReadAsciiStatic(64);

            //packet.Reader.ReadInt32(); // Unknown 4-Bytes (Maybe an int?)
        }

        /// <summary>
        ///     Method for handling the actual logic of the packet
        /// </summary>
        /// <param name="packet">The packet that got received</param>
        /// <param name="connection">The mysql connection</param>
        public static void OnPacket(Packet packet, MySqlConnection connection)
        {
            var authPacket = new UserAuthPacket(packet);

            Log.Debug("Login (v{0}) request from {1}", authPacket.ProtocolVersion.ToString(), authPacket.Username);

            // Check the protocol version, make sure the client is up-to-date with us
            if (authPacket.ProtocolVersion < ServerMain.ProtocolVersion)
            {
                Log.Debug("Client too old?");
                packet.Sender.SendError("Your client is outdated!");
            }

            // Check if the account exists
            if (!AccountModel.AccountExists(connection, authPacket.Username))
            {
                Log.Debug("Account {0} not found!", authPacket.Username);
                packet.Sender.SendError("Invalid Username or password!");
                return;
            }

            // Retrieve the account
            var user = AccountModel.Retrieve(connection, authPacket.Username);
            if (user == null)
            {
                Log.Debug("Account {0} not found!", authPacket.Username);
                packet.Sender.SendError("Invalid Username or password!");
                return;
            }

            // Check password
            var passwordHashed = Util.Password.GenerateSaltedHash(authPacket.Password, user.Salt);
            if (passwordHashed != user.Password)
            {
                Log.Debug("Account {0} found but invalid password! (Entered PW: {1} (Salt: {2}) vs SaltedPW: {3})",
                    authPacket.Username,
                    passwordHashed, user.Salt, user.Password);
                packet.Sender.SendError("Invalid Username or password!");
                return;
            }

            // Create new session ticket
            var ticket = AccountModel.CreateSession(connection, authPacket.Username);
            
            packet.Sender.Send(new UserAuthAnswerPacket
            {
                Ticket = ticket,
                Servers = ServerModel.Retrieve(connection).ToArray()
            }.CreatePacket());
        }
    }
}