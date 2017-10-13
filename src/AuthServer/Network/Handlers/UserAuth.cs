using System;
using System.Net;
using Shared;
using Shared.Models;
using Shared.Network;
using Shared.Network.AuthServer;
using Shared.Objects;
using Shared.Util;

namespace AuthServer.Network.Handlers
{
    public static class UserAuth
    {
        /// <summary>
        /// Packet when a user tries to login
        /// </summary>
        /// <param name="packet">The packet</param>
        [Packet(Packets.CmdUserAuth)]
        public static void Handle(Packet packet)
        {
            var authPacket = new UserAuthPacket(packet);

            Log.Debug("Login (v{0}) request from {1}", authPacket.ProtocolVersion.ToString(), authPacket.Username);

            // Check the protocol version, make sure the client is up-to-date with us
            if (authPacket.ProtocolVersion < ServerMain.ProtocolVersion)
            {
                Log.Debug("Client too old?");
                packet.Sender.SendError("Your client is outdated!");
            }

            // Retrieve the account if it exists
            var user = AccountModel.Retrieve(AuthServer.Instance.Database.Connection, authPacket.Username);
            if (user == null)
            {
                if (!AuthServer.Instance.Config.Auth.NewAccountsLogin)
                {
                    Log.Debug("Account {0} not found!", authPacket.Username);
                    packet.Sender.SendError("Invalid Username or password!");
                    return;
                }
                
                var uid = AccountModel.CreateAccount(AuthServer.Instance.Database.Connection,
                    packet.Sender.EndPoint.Address.ToString(), authPacket.Username, authPacket.Password);
                user = AccountModel.Retrieve(AuthServer.Instance.Database.Connection, (ulong) uid);
            }

            // Check if user is banned
            if (user.IsUserBanned())
            {
                if (user.BanValidUntil != 0)
                    packet.Sender.SendError($"Your account is suspended until {DateTimeOffset.FromUnixTimeSeconds(user.BanValidUntil)}");
                else
                    packet.Sender.SendError("Your account was banned!");
                
                packet.Sender.KillConnection("Banned user");
                return;
            }

            // Check password
            if(!user.CheckPassword(authPacket.Password))
            {
                packet.Sender.SendError("Invalid Username or password!");
                return;
            }
            
            // Create session ticket.
            user.Ticket = User.CreateSessionTicket();
            if (!AccountModel.SetSessionTicket(AuthServer.Instance.Database.Connection, user))
            {
                packet.Sender.SendError("There was an error logging you in.");
                packet.Sender.KillConnection("Failed to create session ticket!");
                return;
            }
            
            packet.Sender.Send(new UserAuthAnswerPacket
            {
                Ticket = user.Ticket,
                Servers = new Server[1]{
                    new Server
                    {
                        ServerName = "DCNC",
                        ServerId = 1,
                        PlayerCount = 0.0f,
                        MaxPlayers = 7000.0f,
                        ServerState = 1,
                        GameTime = Environment.TickCount,
                        LobbyTime = Environment.TickCount,
                        Area1Time = Environment.TickCount,
                        Area2Time = Environment.TickCount,
                        RankingUpdateTime = Environment.TickCount,
                        GameServerIp = IPAddress.Parse(AuthServer.Instance.Config.Ip.GameServerIp).GetAddressBytes(),
                        LobbyServerIp = IPAddress.Parse(AuthServer.Instance.Config.Ip.LobbyServerIp).GetAddressBytes(),
                        AreaServer1Ip = IPAddress.Parse(AuthServer.Instance.Config.Ip.AreaServer1Ip).GetAddressBytes(),
                        AreaServer2Ip = IPAddress.Parse(AuthServer.Instance.Config.Ip.AreaServer2Ip).GetAddressBytes(),
                        RankingServerIp = IPAddress.Parse(AuthServer.Instance.Config.Ip.RankingServerIp).GetAddressBytes(),
                        GameServerPort = (ushort)AuthServer.Instance.Config.Ip.GameServerPort,
                        LobbyServerPort = (ushort)AuthServer.Instance.Config.Ip.LobbyServerPort,
                        AreaServerPort = (ushort)AuthServer.Instance.Config.Ip.AreaServer1Port,
                        AreaServer2Port = (ushort)AuthServer.Instance.Config.Ip.AreaServer2Port,
                        AreaServerUdpPort = (ushort)AuthServer.Instance.Config.Ip.AreaServer1UdpPort,
                        AreaServer2UdpPort = (ushort)AuthServer.Instance.Config.Ip.AreaServer2UdpPort,
                        RankingServerPort = (ushort)AuthServer.Instance.Config.Ip.RankingServerPort
                    }
                }
            }.CreatePacket());
        }
    }
}