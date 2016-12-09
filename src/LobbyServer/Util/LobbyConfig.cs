using Shared.Util.Configuration;

namespace LobbyServer.Util
{
    public class LobbyConf : BaseConf
    {
        /// <summary>
        /// login.conf
        /// </summary>
        public LobbyConfFile Lobby { get; protected set; }

        public LobbyConf()
        {
            Lobby = new LobbyConfFile();
        }

        public override void Load()
        {
            LoadDefault();
            Lobby.Load();
        }
    }

    /// <summary>
    /// Represents login.conf
    /// </summary>
    public class LobbyConfFile : ConfFile
    {
        public int Port { get; protected set; }

        public void Load()
        {
            Require("system/conf/lobby.conf");

            Port = GetInt("port", 11011);
        }
    }
}
