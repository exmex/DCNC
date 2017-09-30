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
    public static class Authentication
    {
        /// <summary>
        /// Handles the CmdServerMessage packet
        /// </summary>
        /// <param name="packet">The packet</param>
        [Packet(Packets.CmdServerMessage)]
        public static void ServerMessage(Packet packet)
        {
            /*Ignored*/
        }

        /// <summary>
        /// Packet when a user tries to login
        /// </summary>
        /// <param name="packet">The packet</param>
        [Packet(Packets.CmdUserAuth)]
        public static void UserAuth(Packet packet)
        {
            var authPacket = new UserAuthPacket(packet);

            Log.Debug("Login (v{0}) request from {1}", authPacket.ProtocolVersion.ToString(), authPacket.Username);

            // Check the protocol version, make sure the client is up-to-date with us
            if (authPacket.ProtocolVersion < ServerMain.ProtocolVersion)
            {
                Log.Debug("Client too old?");
                packet.Sender.SendError("Your client is outdated!");
            }

            // Check if the account exists <-- Should be handled by Retrieve!
            /*if (!AccountModel.AccountExists(AuthServer.Instance.Database.Connection, authPacket.Username))
            {
                Log.Debug("Account {0} not found!", authPacket.Username);
                packet.Sender.SendError("Invalid Username or password!");
                return;
            }*/

            // Retrieve the account
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
                user = AccountModel.Retrieve(AuthServer.Instance.Database.Connection, (ulong)uid);
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
            user.Ticket = user.CreateSessionTicket();
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
        
        /*
        [Packet(Packets.CmdServerMessage)]
        public static void ServerMessage(Packet packet)
        {
            // TODO: Send serverlist here
            var pkt = new Packet(23);
            var servers = new Server[1];
            servers[0] = new Server
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

            pkt.Writer.Write(servers.Length); // Size
            //for int i = 0; i < size; i++
            for (var i = 0; i < servers.Length; i++)
            {
                pkt.Writer.WriteUnicodeStatic(servers[i].ServerName, 32); // 32
                pkt.Writer.Write(servers[i].ServerId);
                pkt.Writer.Write(servers[i].PlayerCount);
                pkt.Writer.Write(servers[i].MaxPlayers);
                pkt.Writer.Write(servers[i].ServerState);
                if (servers[i].ServerState == 100)
                {
                    pkt.Writer.Write(0);
                    pkt.Writer.Write(0);
                    pkt.Writer.Write(0);
                    pkt.Writer.Write(0);
                }
                else
                {
                    pkt.Writer.Write(servers[i].GameTime);
                    pkt.Writer.Write(servers[i].LobbyTime);
                    pkt.Writer.Write(servers[i].Area1Time);
                    pkt.Writer.Write(servers[i].Area2Time);
                }
                pkt.Writer.Write(servers[i].RankingUpdateTime);
                pkt.Writer.Write(servers[i].GameServerIp);
                pkt.Writer.Write(servers[i].LobbyServerIp);
                pkt.Writer.Write(servers[i].AreaServer1Ip);
                pkt.Writer.Write(servers[i].AreaServer2Ip);
                pkt.Writer.Write(servers[i].RankingServerIp);
                pkt.Writer.Write(servers[i].GameServerPort);
                pkt.Writer.Write(servers[i].LobbyServerPort);
                pkt.Writer.Write(servers[i].AreaServerPort);
                pkt.Writer.Write(servers[i].AreaServer2Port);
                pkt.Writer.Write(servers[i].AreaServerUdpPort);
                pkt.Writer.Write(servers[i].AreaServer2UdpPort);
                pkt.Writer.Write(servers[i].RankingServerPort);
            }
            packet.Sender.Send(pkt);

            var serverId = packet.Reader.ReadInt32();

            var ack = new Packet(Packets.ServerMessageAck);
            if (serverId != 0)
            {
                ack.Writer.Write(serverId);
                ack.Writer.WriteUnicode("Hello world! This is a basic server message!");
            }
            else
            {
                ack.Writer.Write(serverId);
                ack.Writer.Write(0);
            }
            packet.Sender.Send(ack);
        }
        
        [Packet(Packets.CmdUserAuth)]
        public static void UserAuth(Packet packet)
        {
            var authPacket = new UserAuthPacket(packet);

            Log.Debug("Login (v{0}) request from {1}", authPacket.ProtocolVersion.ToString(), authPacket.Username);

            if (authPacket.ProtocolVersion < ServerMain.ProtocolVersion)
            {
                Log.Debug("Client too old?");
                packet.Sender.Error("Your client is outdated!");
            }

            if (!AccountModel.AccountExists(AuthServer.Instance.Database.Connection, authPacket.Username))
            {
                Log.Debug("Account {0} not found!", authPacket.Username);
                packet.Sender.Error("Invalid Username or password!");
                return;
            }

            var user = AccountModel.Retrieve(AuthServer.Instance.Database.Connection, authPacket.Username);
            if (user == null)
            {
                Log.Debug("Account {0} not found!", authPacket.Username);
                packet.Sender.Error("Invalid Username or password!");
                return;
            }
            var passwordHashed = Password.GenerateSaltedHash(authPacket.Password, user.Salt);
            if (passwordHashed != user.Password)
            {
                Log.Debug("Account {0} found but invalid password! ({1} ({2}) vs {3})", authPacket.Username,
                    passwordHashed, user.Salt, user.Password);
                packet.Sender.Error("Invalid Username or password!");
                return;
            }

            var ticket = AccountModel.CreateSession(AuthServer.Instance.Database.Connection, authPacket.Username);

            // Wrong protocol -> 20070

            *var ack = new Packet(Packets.UserAuthAck);
            packet.Sender.Error("Invalid Username or password!");*

            var ack = new UserAuthAnswerPacket
            {
                Ticket = ticket,
                Servers = ServerModel.Retrieve(AuthServer.Instance.Database.Connection).ToArray()
            };

            packet.Sender.Send(ack.CreatePacket());
        }*/
    }
}