using System;
using System.Collections.Generic;
using GameServer.Database;
using GameServer.Util;
using Shared;
using Shared.Network;
using Shared.Objects;
using Shared.Util;
using TdfReader = Shared.Util.TdfReader;

namespace GameServer
{
    public class GameServer : ServerMain
    {
        public static readonly GameServer Instance = new GameServer();

        private bool _running;

        /// <summary>
        /// Instance of the actual server component.
        /// </summary>
        public DefaultServer Server { get; set; }

        /// <summary>
        /// Database
        /// </summary>
        public GameDatabase Database { get; private set; }

        /// <summary>
        /// Configuration
        /// </summary>
        private GameConf Config { get; set; }

        public static Dictionary<uint, XiStrQuest> QuestTable;
        public static Dictionary<long, long> LevelTable;

        /// <summary>
        /// Initializes fields and properties
        /// </summary>
        private GameServer()
        {
        }

        /// <summary>
        /// Loads all necessary components and starts the server.
        /// </summary>
        public void Run()
        {
            if (_running)
                throw new Exception("Server is already running.");

            int x, y, width, height;
            GetWindowPosition(out x, out y, out width, out height);
            SetWindowPosition(width + 5, 0, width, height);

            ConsoleUtil.WriteHeader("Game Server", ConsoleColor.DarkGreen);
            ConsoleUtil.LoadingTitle();

            NavigateToRoot();

            // Conf
            LoadConf(Config = new GameConf());

            // Database
            InitDatabase(Database = new GameDatabase(), Config);

            // Data
            TdfReader reader = new TdfReader();
            if (reader.Load("system/data/QuestServer.tdf"))
            {
                Log.Debug("Loading Quest Table");
                QuestTable = XiStrQuest.LoadFromTdf(reader);
                Log.Debug("Quest Table Initialized with {0:D} rows.", QuestTable.Count);
            }else
                Log.Debug("Quest Table Load failed.");

            reader = new TdfReader();
            if (reader.Load("system/data/LevelServer.tdf"))
            {
                Log.Debug("Loading Exp Table");
                LevelTable = XiExpTable.LoadFromTdf(reader);
                Log.Debug("Exp Table Initialized with {0:D} rows.", QuestTable.Count);
            }
            else
                Log.Debug("Exp Table Load failed.");

            // Start
            Server = new DefaultServer(Config.Game.Port);
            Server.Start();

            ConsoleUtil.RunningTitle();
            _running = true;

            // Commands
            var commands = new GameConsoleCommands();
            commands.Wait();
        }
    }
}
