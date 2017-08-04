using System;
using System.Net;

namespace Shared.Network.AuthServer
{
    public class UserAuthAnswerPacket
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

        /// <summary>
        ///     Sends the auth answer packet.
        /// </summary>
        /// <param name="packetId">The packet identifier.</param>
        /// <param name="client">The client to send the packet to.</param>
        public void Send(ushort packetId, Client client)
        {
            var pkt = new Packet(packetId);

            pkt.Writer.Write(Ticket);
            pkt.Writer.Write(Result);
            pkt.Writer.Write(Time);
            pkt.Writer.Write(new byte[64]); // Filler. Unused?
            pkt.Writer.Write(ServerListId);
            pkt.Writer.Write(ServerCount);
            for (var i = 0; i < Servers.Length; i++)
            {
                pkt.Writer.WriteUnicodeStatic(Servers[i].ServerName, 32); // 32
                pkt.Writer.Write(Servers[i].ServerId);
                pkt.Writer.Write(Servers[i].PlayerCount);
                pkt.Writer.Write(Servers[i].MaxPlayers);
                pkt.Writer.Write(Servers[i].ServerState);
                pkt.Writer.Write(Servers[i].GameTime);
                pkt.Writer.Write(Servers[i].LobbyTime);
                pkt.Writer.Write(Servers[i].Area1Time);
                pkt.Writer.Write(Servers[i].Area2Time);
                pkt.Writer.Write(Servers[i].RankingUpdateTime);
                pkt.Writer.Write(Servers[i].GameServerIp);
                pkt.Writer.Write(Servers[i].LobbyServerIp);
                pkt.Writer.Write(Servers[i].AreaServer1Ip);
                pkt.Writer.Write(Servers[i].AreaServer2Ip);
                pkt.Writer.Write(Servers[i].RankingServerIp);
                pkt.Writer.Write(Servers[i].GameServerPort);
                pkt.Writer.Write(Servers[i].LobbyServerPort);
                pkt.Writer.Write(Servers[i].AreaServerPort);
                pkt.Writer.Write(Servers[i].AreaServer2Port);
                pkt.Writer.Write(Servers[i].AreaServerUdpPort);
                pkt.Writer.Write(Servers[i].AreaServer2UdpPort);
                pkt.Writer.Write(Servers[i].RankingServerPort);
            }

            client.Send(pkt);
        }

        public struct Server
        {
            public string ServerName;
            public uint ServerId;
            public float PlayerCount;
            public float MaxPlayers;
            public int ServerState; // 100 maintenance?
            public int GameTime;
            public int LobbyTime;
            public int Area1Time;
            public int Area2Time;
            public int RankingUpdateTime;
            public byte[] GameServerIp;
            public byte[] LobbyServerIp;
            public byte[] AreaServer1Ip;
            public byte[] AreaServer2Ip;
            public byte[] RankingServerIp;
            public ushort GameServerPort;
            public ushort LobbyServerPort;
            public ushort AreaServerPort;
            public ushort AreaServer2Port;
            public ushort AreaServerUdpPort;
            public ushort AreaServer2UdpPort;
            public ushort RankingServerPort;
        }
    }
}