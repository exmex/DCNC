using System;
using System.Net;
using MySql.Data.MySqlClient;
using Shared.Models;
using Shared.Objects;
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
            
        }
    }
}