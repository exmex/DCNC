using System.IO;
using NUnit.Framework;
using Shared;
using Shared.Network.AuthServer;
using Shared.Util;

namespace SharedTests.Packets
{
    [TestFixture]
    public class AuthServerTest
    {
        #region "UserAuth Exchange"
        [Test]
        public void UserAuthPacketTest()
        {
            var authPacket = new UserAuthPacket(Utilities.ConstructTestPacket("UserAuth.bin", Shared.Network.Packets.CmdUserAuth));
            
            Assert.AreEqual(authPacket.ProtocolVersion, ServerMain.ProtocolVersion);
            
            Assert.AreEqual("admin", authPacket.Username);
            Assert.AreEqual("admin", authPacket.Password);
        }

        [Test]
        public void UserAuthAnswerPacket()
        {
            var packet = new UserAuthAnswerPacket();
            var bytes = packet.GetBytes();

            using (var ms = new MemoryStream(bytes))
            {
                using (var bs = new BinaryReaderExt(ms))
                {
                    var ticket = bs.ReadUInt32();
                    Assert.AreEqual(packet.Ticket, ticket);
                    
                    var result = bs.ReadInt32();
                    Assert.AreEqual(packet.Result, result);
                    
                    var time = bs.ReadInt32();
                    Assert.AreEqual(packet.Time, time);
                    
                    bs.ReadBytes(64); // Filler
                    
                    var serverListId = bs.ReadUInt16();
                    Assert.AreEqual(packet.ServerListId, serverListId);
                    
                    var serverCount = bs.ReadInt32();
                    Assert.AreEqual(packet.ServerCount, serverCount);
                    
                    // Normally we could read all servers, but for tests only the first
                    // is interesting
                    var serverName = bs.ReadUnicodeStatic(32);
                    StringAssert.AreEqualIgnoringCase(serverName, packet.Servers[0].ServerName);
                    
                    var serverId = bs.ReadUInt32();
                    Assert.AreEqual(packet.Servers[0].ServerId, serverId);
                    
                    var playerCount = bs.ReadSingle();
                    Assert.AreEqual(packet.Servers[0].PlayerCount, playerCount);
                    
                    var maxPlayers = bs.ReadSingle();
                    Assert.AreEqual(packet.Servers[0].MaxPlayers, maxPlayers);
                    
                    var serverState = bs.ReadInt32();
                    Assert.AreEqual(packet.Servers[0].ServerState, serverState);
                    
                    var gameTime = bs.ReadInt32();
                    Assert.AreEqual(packet.Servers[0].GameTime, gameTime);
                    
                    var lobbyTime = bs.ReadInt32();
                    Assert.AreEqual(packet.Servers[0].LobbyTime, lobbyTime);
                    
                    var area1Time = bs.ReadInt32();
                    Assert.AreEqual(packet.Servers[0].Area1Time, area1Time);
                    
                    var area2Time = bs.ReadInt32();
                    Assert.AreEqual(packet.Servers[0].Area2Time, area2Time);
                    
                    var rankingUpdateTime = bs.ReadInt32();
                    Assert.AreEqual(packet.Servers[0].RankingUpdateTime, rankingUpdateTime);
                    
                    var gameServerIp = bs.ReadBytes(4);
                    Assert.AreEqual(packet.Servers[0].GameServerIp, gameServerIp);
                    
                    var lobbyServerIp = bs.ReadBytes(4);
                    Assert.AreEqual(packet.Servers[0].LobbyServerIp, lobbyServerIp);
                    
                    var areaServer1Ip = bs.ReadBytes(4);
                    Assert.AreEqual(packet.Servers[0].AreaServer1Ip, areaServer1Ip);
                    
                    var areaServer2Ip = bs.ReadBytes(4);
                    Assert.AreEqual(packet.Servers[0].AreaServer2Ip, areaServer2Ip);
                    
                    var rankingServerIp = bs.ReadBytes(4);
                    Assert.AreEqual(packet.Servers[0].RankingServerIp, rankingServerIp);
                    
                    var gameServerPort = bs.ReadUInt16();
                    Assert.AreEqual(packet.Servers[0].GameServerPort, gameServerPort);
                    
                    var lobbyServerPort = bs.ReadUInt16();
                    Assert.AreEqual(packet.Servers[0].LobbyServerPort, lobbyServerPort);
                    
                    var areaServerPort = bs.ReadUInt16();
                    Assert.AreEqual(packet.Servers[0].AreaServerPort, areaServerPort);
                    
                    var areaServer2Port = bs.ReadUInt16();
                    Assert.AreEqual(packet.Servers[0].AreaServer2Port, areaServer2Port);
                    
                    var areaServerUdpPort = bs.ReadUInt16();
                    Assert.AreEqual(packet.Servers[0].AreaServerUdpPort, areaServerUdpPort);
                    
                    var areaServer2UdpPort = bs.ReadUInt16();
                    Assert.AreEqual(packet.Servers[0].AreaServer2UdpPort, areaServer2UdpPort);
                    
                    var rankingServerPort = bs.ReadUInt16();
                    Assert.AreEqual(packet.Servers[0].RankingServerPort, rankingServerPort);
                }
            }
        }
        #endregion
    }
}