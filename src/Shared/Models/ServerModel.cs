using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net;
using MySql.Data.MySqlClient;
using Shared.Network.AuthServer;

namespace Shared.Models
{
    public static class ServerModel
    {
        public static List<UserAuthAnswerPacket.Server> Retrieve(MySqlConnection dbconn)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM servers", dbconn);

            List<UserAuthAnswerPacket.Server> servers = new List<UserAuthAnswerPacket.Server>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var server = new UserAuthAnswerPacket.Server
                    {
                        ServerName = reader["Name"] as string,
                        ServerId = (uint)servers.Count,
                        PlayerCount = Convert.ToSingle(reader["PlayersOnline"]),
                        MaxPlayers = Convert.ToSingle(reader["MaxPlayers"]),
                        ServerState = 1,
                        GameTime = Environment.TickCount,
                        LobbyTime = Environment.TickCount,
                        Area1Time = Environment.TickCount,
                        Area2Time = Environment.TickCount,
                        RankingUpdateTime = Environment.TickCount,
                        GameServerIp = IPAddress.Parse((string) reader["GameServerIp"]).GetAddressBytes(),
                        LobbyServerIp = IPAddress.Parse((string) reader["LobbyServerIp"]).GetAddressBytes(),
                        AreaServer1Ip = IPAddress.Parse((string) reader["AreaServer1Ip"]).GetAddressBytes(),
                        AreaServer2Ip = IPAddress.Parse((string) reader["AreaServer2Ip"]).GetAddressBytes(),
                        RankingServerIp = IPAddress.Parse((string) reader["RankingServerIp"]).GetAddressBytes(),
                        GameServerPort = Convert.ToUInt16(reader["GameServerPort"]),
                        LobbyServerPort = Convert.ToUInt16(reader["LobbyServerPort"]),
                        AreaServerPort = Convert.ToUInt16(reader["AreaServer1Port"]),
                        AreaServer2Port = Convert.ToUInt16(reader["AreaServer2Port"]),
                        AreaServerUdpPort = Convert.ToUInt16(reader["AreaServer1UdpPort"]),
                        AreaServer2UdpPort = Convert.ToUInt16(reader["AreaServer2UdpPort"]),
                        RankingServerPort = Convert.ToUInt16(reader["RankingServerPort"])
                    };
                    servers.Add(server);
                }
            }

            return servers;
        }
    }
}
