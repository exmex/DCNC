namespace Shared.Objects
{
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