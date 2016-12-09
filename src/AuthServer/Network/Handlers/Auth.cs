using System;
using System.Net;
using Shared;
using Shared.Models;
using Shared.Network;
using Shared.Objects;
using Shared.Util;

namespace AuthServer.Network.Handlers
{
    public static class Authentication
    {
        public class UserAuthAck
        {
            private const uint ticket = 0;
            private const int result = 0;
            private static int time = Environment.TickCount;
            private static byte[] filler = new byte[64]; // m_STicket unused thought?
            private const ushort serverListId = 23;
            private const int serverCount = 1;
            private const string serverName = "TEST"; // 32
            private const uint serverId = 1;
            private const float playerCount = 0.0f;
            private const float maxPlayers = 7000.0f;
            private const int serverState = 1; // 100 maintenance?
            private static int gameTime = Environment.TickCount;
            private static int lobbyTime = Environment.TickCount;
            private static int area1Time = Environment.TickCount;
            private static int area2Time = Environment.TickCount;
            private static int rankingUpdateTime = Environment.TickCount;
            private static byte[] gameServerIp = IPAddress.Parse("127.0.0.1").GetAddressBytes();
            private static byte[] lobbyServerIp = IPAddress.Parse("127.0.0.1").GetAddressBytes();
            private static byte[] areaServer1Ip = IPAddress.Parse("127.0.0.1").GetAddressBytes();
            private static byte[] areaServer2Ip = IPAddress.Parse("127.0.0.1").GetAddressBytes();
            private static byte[] rankingServerIp = IPAddress.Parse("127.0.0.1").GetAddressBytes();
            const ushort gameServerPort = 11021;
            const ushort lobbyServerPort = 11011;
            const ushort areaServerPort = 11031;
            const ushort areaServer2Port = 11041;
            const ushort areaServerUdpPort = 10701;
            const ushort areaServer2UdpPort = 10702;
            const ushort rankingServerPort = 11078;

            public static void Serialize(uint _ticket, PacketWriter writer)
            {
                writer.Write(_ticket);
                writer.Write(result);
                writer.Write(time);
                writer.Write(filler); // figure out
                writer.Write(serverListId);
                writer.Write(serverCount);
                writer.WriteUnicodeStatic(serverName, 32); // 32
                writer.Write(serverId);
                writer.Write(playerCount);
                writer.Write(maxPlayers);
                writer.Write(serverState);
                writer.Write(gameTime);
                writer.Write(lobbyTime);
                writer.Write(area1Time);
                writer.Write(area2Time);
                writer.Write(rankingUpdateTime);
                writer.Write(gameServerIp);
                writer.Write(lobbyServerIp);
                writer.Write(areaServer1Ip);
                writer.Write(areaServer2Ip);
                writer.Write(rankingServerIp);
                writer.Write(gameServerPort);
                writer.Write(lobbyServerPort);
                writer.Write(areaServerPort);
                writer.Write(areaServer2Port);
                writer.Write(areaServerUdpPort);
                writer.Write(areaServer2UdpPort);
                writer.Write(rankingServerPort);
            }
        }

        [Packet(Packets.CmdUserAuth)]
        public static void UserAuth(Packet packet)
        {
            int version = packet.Reader.ReadInt32();
            var username = packet.Reader.ReadUnicodeStatic(40);
            var password = packet.Reader.ReadAscii();
            password = password.Substring(0, password.Length - 1);

            Log.Debug("Login (v{0}) request from {1}", version.ToString(), username);

            if (!AccountModel.AccountExists(AuthServer.Instance.Database.Connection, username))
            {
                Log.Debug("Account {0} not found!", username);
                packet.Sender.Error("Invalid Username or password!");
                return;
            }

            User user = AccountModel.Retrieve(AuthServer.Instance.Database.Connection, username);
            if (user == null)
            {
                Log.Debug("Account {0} not found!", username);
                packet.Sender.Error("Invalid Username or password!");
                return;
            }
            var passwordHashed = Password.GenerateSaltedHash(password, user.Salt);
            if(passwordHashed != user.Password)
            {
                Log.Debug("Account {0} found but invalid password! ({1} ({2}) vs {3})", username, passwordHashed, user.Salt, user.Password);
                packet.Sender.Error("Invalid Username or password!");
                return;
            }

            uint ticket = AccountModel.CreateSession(AuthServer.Instance.Database.Connection, username);

            // Wrong protocol -> 20070

            /*var ack = new Packet(Packets.UserAuthAck);
            packet.Sender.Error("Invalid Username or password!");*/

            var ack = new Packet(Packets.UserAuthAck);

            UserAuthAck.Serialize(ticket, ack.Writer);

            packet.Sender.Send(ack);
        }
    }
}

