using Shared.Util.Configuration;

namespace GameServer.Util
{
    public class GameConf : BaseConf
    {
        public GameConf()
        {
            Game = new GameConfFile();
        }

        /// <summary>
        ///     login.conf
        /// </summary>
        public GameConfFile Game { get; protected set; }

        public override void Load()
        {
            LoadDefault();
            Game.Load();
        }
    }

    /// <summary>
    ///     Represents login.conf
    /// </summary>
    public class GameConfFile : ConfFile
    {
        public int Port { get; protected set; }

        public int CrewCreationCost { get; protected set; }

        public int CrewCreationMinLevel { get; protected set; }

        public void Load()
        {
            Require("system/conf/game.conf");

            Port = GetInt("port", 11021);

            CrewCreationCost = GetInt("crewCreationCost", 10000);
            
            CrewCreationMinLevel = GetInt("crewCreationMinLevel", 20);
        }
    }
}