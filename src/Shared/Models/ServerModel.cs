using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net;
using MySql.Data.MySqlClient;
using Shared.Objects;

namespace Shared.Models
{
    /// <summary>
    /// TODO: No need to refetch this! We could cache this since we're never changing this when the server is running! 
    /// </summary>
    public static class ServerModel
    {
        /// <summary>
        /// Retrieves the serverlist from DB
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <returns>List containing all servers</returns>
        public static List<Server> Retrieve(MySqlConnection dbconn)
        {
            var command = new MySqlCommand("SELECT * FROM servers", dbconn);

            var servers = new List<Server>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var server = new Server
                    {
                        ServerName = reader["Name"] as string,
                        ServerId = (uint) servers.Count,
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