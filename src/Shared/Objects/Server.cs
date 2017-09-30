namespace Shared.Objects
{
    public struct Server
    {
        /// <summary>
        /// The name of the server
        /// </summary>
        public string ServerName;
        
        /// <summary>
        /// The id of the server
        /// </summary>
        public uint ServerId;
        
        /// <summary>
        /// The current player count of the server
        /// </summary>
        public float PlayerCount;
        
        /// <summary>
        /// How many players can fit into this server
        /// </summary>
        public float MaxPlayers;
        
        /// <summary>
        /// The state of the server
        /// 100 = Maintenance
        /// </summary>
        public int ServerState;
        
        /// <summary>
        /// The current gameserver time of the server
        /// </summary>
        public int GameTime;
        
        /// <summary>
        /// The current lobby time of the server
        /// </summary>
        public int LobbyTime;
        
        /// <summary>
        /// The current area time
        /// </summary>
        public int Area1Time;
        
        /// <summary>
        /// The current area 2 time
        /// </summary>
        public int Area2Time;
        
        /// <summary>
        /// The last ranking update time
        /// </summary>
        public int RankingUpdateTime;
        
        /// <summary>
        /// The IP of the GameServer
        /// </summary>
        public byte[] GameServerIp;
        
        /// <summary>
        /// The IP of the LobbyServer
        /// </summary>
        public byte[] LobbyServerIp;
        
        /// <summary>
        /// The IP of the AreaServer1
        /// </summary>
        public byte[] AreaServer1Ip;
        
        /// <summary>
        /// The IP of the AreaServer2
        /// </summary>
        public byte[] AreaServer2Ip;
        
        /// <summary>
        /// The IP of the Ranking Server
        /// </summary>
        public byte[] RankingServerIp;
        
        /// <summary>
        /// The port of the GameServer
        /// </summary>
        public ushort GameServerPort;
        
        /// <summary>
        /// The port of the LobbyServer
        /// </summary>
        public ushort LobbyServerPort;
        
        /// <summary>
        /// The port of the AreaServer
        /// </summary>
        public ushort AreaServerPort;
        
        /// <summary>
        /// The port of the AreaServer2
        /// </summary>
        public ushort AreaServer2Port;
        
        /// <summary>
        /// The UDP port of the AreaServer 
        /// </summary>
        public ushort AreaServerUdpPort;
        
        /// <summary>
        /// The UDP port of the AreaServer2
        /// </summary>
        public ushort AreaServer2UdpPort;
        
        /// <summary>
        /// The port of the RankingServer
        /// </summary>
        public ushort RankingServerPort;
    }
}