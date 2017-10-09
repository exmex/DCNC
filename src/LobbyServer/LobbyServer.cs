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
        ///     Initializes fields and properties
        /// </summary>
        private LobbyServer()
        {
        }

        /// <summary>
        ///     Instance of the actual server component.
        /// </summary>
        public DefaultServer Server { get; set; }

        /// <summary>
        ///     Database
        /// </summary>
        public LobbyDatabase Database { get; private set; }

        /// <summary>
        ///     Configuration
        /// </summary>
        public LobbyConf Config { get; set; }

        /// <summary>
        ///     Loads all necessary components and starts the server.
        /// </summary>
        public void Run()
        {
            if (_running)
                throw new Exception("Server is already running.");

            int x, y, width, height;
            Win32.GetWindowPosition(out x, out y, out width, out height);
            Win32.SetWindowPosition(0, height + 5, width, height);

            ConsoleUtil.WriteHeader($"Lobby Server ({Shared.Util.Version.GetVersion()})", ConsoleColor.DarkGreen);
            ConsoleUtil.LoadingTitle();

            Log.Info("Server startup requested");
            Log.Info($"Server Version {Shared.Util.Version.GetVersion()}");

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