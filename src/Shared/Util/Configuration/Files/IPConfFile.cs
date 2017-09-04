namespace Shared.Util.Configuration.Files
{
    public class IPConfFile : ConfFile
    {
        
        public string GameServerIp { get; protected set; }
        public string LobbyServerIp { get; protected set; }
        public string AreaServer1Ip { get; protected set; }
        public string AreaServer2Ip { get; protected set; }
        public string RankingServerIp { get; protected set; }
        
        public int GameServerPort { get; protected set; }
        public int LobbyServerPort { get; protected set; }
        public int AreaServer1Port { get; protected set; }
        public int AreaServer1UdpPort { get; protected set; }
        public int AreaServer2Port { get; protected set; }
        public int AreaServer2UdpPort { get; protected set; }
        public int RankingServerPort { get; protected set; }
        
        public void Load()
        {
            Require("system/conf/game.conf");
            
            GameServerIp = GetString("gameServerIp", "127.0.0.1");
            LobbyServerIp = GetString("lobbyServerIp", "127.0.0.1");
            AreaServer1Ip = GetString("areaServer1Ip", "127.0.0.1");
            AreaServer2Ip = GetString("areaServer2Ip", "127.0.0.1");
            RankingServerIp = GetString("rankingServerIp", "127.0.0.1");

            GameServerPort = GetInt("gameServerPort", 11021);
            LobbyServerPort = GetInt("lobbyServerPort", 11011);
            AreaServer1Port = GetInt("areaServer1Port", 11031);
            AreaServer1UdpPort = GetInt("areaServer1UdpPort", 10701);
            AreaServer2Port = GetInt("areaServer2Port", 11041);
            AreaServer2UdpPort = GetInt("areaServer2UdpPort", 10702);
            RankingServerPort = GetInt("lobbyServerPort", 11078);
        }
    }
}