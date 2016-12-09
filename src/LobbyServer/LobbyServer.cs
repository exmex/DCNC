using System;
using LobbyServer.Database;
using LobbyServer.Util;
using Shared;
using Shared.Network;
using Shared.Util;

namespace LobbyServer
{
    public class LobbyServer : ServerMain
    {
        public static readonly LobbyServer Instance = new LobbyServer();

        private bool _running;

        /// <summary>
        /// Instance of the actual server component.
        /// </summary>
        private DefaultServer Server { get; set; }

        /// <summary>
        /// Database
        /// </summary>
        public LobbyDatabase Database { get; private set; }

        /// <summary>
        /// Configuration
        /// </summary>
        private LobbyConf Config { get; set; }

        /// <summary>
        /// Initializes fields and properties
        /// </summary>
        private LobbyServer()
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
            SetWindowPosition(0, height + 5, width, height);

            ConsoleUtil.WriteHeader("Lobby Server", ConsoleColor.DarkGreen);
            ConsoleUtil.LoadingTitle();

            NavigateToRoot();

            // Conf
            LoadConf(Config = new LobbyConf());

            // Database
            InitDatabase(Database = new LobbyDatabase(), Config);

            // Start
            Server = new DefaultServer(Config.Lobby.Port);
            Server.Start();

            ConsoleUtil.RunningTitle();
            _running = true;

            // Commands
            var commands = new LobbyConsoleCommands();
            commands.Wait();
        }
    }
}
