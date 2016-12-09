using System;
using GameServer.Database;
using GameServer.Util;
using Shared;
using Shared.Network;
using Shared.Util;

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
