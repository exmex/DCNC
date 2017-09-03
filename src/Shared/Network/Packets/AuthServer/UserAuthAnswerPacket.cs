using System;
using System.IO;
using System.Net;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.AuthServer
{
    public class UserAuthAnswerPacket : OutPacket
    {
        /// <summary>
        ///     The result code
        /// </summary>
        public int Result;

        /// <summary>
        ///     The total count of servers
        /// </summary>
        public int ServerCount; // Could also be replaces with Servers.Length.

        /// <summary>
        ///     The server list index
        /// </summary>
        public ushort ServerListId;

        /// <summary>
        ///     An array containting ServerCount servers
        /// </summary>
        public Server[] Servers;

        /// <summary>
        ///     The new user ticket
        /// </summary>
        public uint Ticket;

        /// <summary>
        ///     The current server ticks
        /// </summary>
        public int Time;

        public UserAuthAnswerPacket()
        {
            Ticket = 0;
            Result = 0;
            Time = Environment.TickCount;
            ServerListId = 23;
            ServerCount = 1;
            Servers = new Server[1];
            Servers[0] = new Server
            {
                ServerName = "Test",
                ServerId = 1,
                PlayerCount = 0.0f,
                MaxPlayers = 7000.0f,
                ServerState = 1,
                GameTime = Environment.TickCount,
                LobbyTime = Environment.TickCount,
                Area1Time = Environment.TickCount,
                Area2Time = Environment.TickCount,
                RankingUpdateTime = Environment.TickCount,
                GameServerIp = IPAddress.Parse("127.0.0.1").GetAddressBytes(),
                LobbyServerIp = IPAddress.Parse("127.0.0.1").GetAddressBytes(),
                AreaServer1Ip = IPAddress.Parse("127.0.0.1").GetAddressBytes(),
                AreaServer2Ip = IPAddress.Parse("127.0.0.1").GetAddressBytes(),
                RankingServerIp = IPAddress.Parse("127.0.0.1").GetAddressBytes(),
                GameServerPort = 11021,
                LobbyServerPort = 11011,
                AreaServerPort = 11031,
                AreaServer2Port = 11041,
                AreaServerUdpPort = 10701,
                AreaServer2UdpPort = 10702,
                RankingServerPort = 11078
            };
        }

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.UserAuthAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Ticket);
                    bs.Write(Result);
                    bs.Write(Time);
                    bs.Write(new byte[64]); // Filler. Unused? ("STicket")
                    bs.Write(ServerListId);
                    bs.Write(ServerCount);
                    for (var i = 0; i < Servers.Length; i++)
                    {
                        bs.WriteUnicodeStatic(Servers[i].ServerName, 32); // 32
                        bs.Write(Servers[i].ServerId);
                        bs.Write(Servers[i].PlayerCount);
                        bs.Write(Servers[i].MaxPlayers);
                        bs.Write(Servers[i].ServerState);
                        bs.Write(Servers[i].GameTime);
                        bs.Write(Servers[i].LobbyTime);
                        bs.Write(Servers[i].Area1Time);
                        bs.Write(Servers[i].Area2Time);
                        bs.Write(Servers[i].RankingUpdateTime);
                        bs.Write(Servers[i].GameServerIp);
                        bs.Write(Servers[i].LobbyServerIp);
                        bs.Write(Servers[i].AreaServer1Ip);
                        bs.Write(Servers[i].AreaServer2Ip);
                        bs.Write(Servers[i].RankingServerIp);
                        bs.Write(Servers[i].GameServerPort);
                        bs.Write(Servers[i].LobbyServerPort);
                        bs.Write(Servers[i].AreaServerPort);
                        bs.Write(Servers[i].AreaServer2Port);
                        bs.Write(Servers[i].AreaServerUdpPort);
                        bs.Write(Servers[i].AreaServer2UdpPort);
                        bs.Write(Servers[i].RankingServerPort);
                    }
                }
                return ms.ToArray();
            }
        }
    }
}