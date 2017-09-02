using System;
using System.Collections.Generic;
using System.IO;
using GameServer.Database;
using GameServer.Util;
using Shared;
using Shared.Network;
using Shared.Objects;
using Shared.Util;

namespace GameServer
{
    public class GameServer : ServerMain
    {
        public static readonly GameServer Instance = new GameServer();

        public static GameChatCommands ChatCommands = new GameChatCommands();

        private bool _running;

        /// <summary>
        ///     Initializes fields and properties
        /// </summary>
        private GameServer()
        {
        }

        /// <summary>
        ///     Instance of the actual server component.
        /// </summary>
        public DefaultServer Server { get; set; }

        /// <summary>
        ///     Database
        /// </summary>
        public GameDatabase Database { get; private set; }

        /// <summary>
        ///     Configuration
        /// </summary>
        private GameConf Config { get; set; }

        /// <summary>
        ///     Loads all necessary components and starts the server.
        /// </summary>
        public void Run()
        {
            if (_running)
                throw new Exception("Server is already running.");

            int x, y, width, height;
            Win32.GetWindowPosition(out x, out y, out width, out height);
            Win32.SetWindowPosition(width + 5, 0, width, height);

            ConsoleUtil.WriteHeader("Game Server", ConsoleColor.DarkGreen);
            ConsoleUtil.LoadingTitle();

            NavigateToRoot();

            // Conf
            LoadConf(Config = new GameConf());

            // Database
            InitDatabase(Database = new GameDatabase(), Config);

            // Data
            var reader = new TdfReader();
            if (reader.Load("system/data/QuestServer.tdf"))
            {
                Log.Debug("Loading Quest Table");
                QuestTable = XiStrQuest.LoadFromTdf(reader);
                if(QuestTable.Count == 0) throw new InvalidDataException("QuestTable corrupt!");
                Log.Debug("Quest Table Initialized with {0:D} rows.", QuestTable.Count);
            }
            else
            {
                Log.Debug("Quest Table Load failed.");
            }
            
            reader = new TdfReader();
            if (reader.Load("system/data/ItemClient.tdf"))
            {
                Log.Debug("Loading Item Table");
                ItemTable = XiStrItem.LoadFromTdf(reader);
                if(ItemTable.Count == 0) throw new InvalidDataException("ItemTable corrupt!");
                Log.Debug("Item Table Initialized with {0:D} rows.", ItemTable.Count);
            }

            // TODO: Load VehicleList.csv to VehicleInfo
            //VehicleInfo.Load("system/data/VehicleList.csv");
            
            reader = new TdfReader();
            if (reader.Load("system/data/LevelServer.tdf"))
            {
                Log.Debug("Loading Exp Table");
                LevelTable = XiExpTable.LoadFromTdf(reader);
                if(LevelTable.Count == 0) throw new InvalidDataException("LevelTable corrupt!");
                Log.Debug("Exp Table Initialized with {0:D} rows.", LevelTable.Count);
            }
            else
            {
                Log.Debug("Exp Table Load failed.");
            }

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