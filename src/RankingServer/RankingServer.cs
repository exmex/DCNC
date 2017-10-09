using System;
using RankingServer.Database;
using RankingServer.Util;
using Shared;
using Shared.Network;
using Shared.Util;

namespace RankingServer
{
    public class RankingServer : ServerMain
    {
        public static readonly RankingServer Instance = new RankingServer();

        private bool _running;

        /// <summary>
        ///     Initializes fields and properties
        /// </summary>
        private RankingServer()
        {
        }

        /// <summary>
        ///     Instance of the actual server component.
        /// </summary>
        private DefaultServer Server { get; set; }

        /// <summary>
        ///     Database
        /// </summary>
        public RankingDatabase Database { get; private set; }

        /// <summary>
        ///     Configuration
        /// </summary>
        private RankingConf Config { get; set; }

        /// <summary>
        ///     Loads all necessary components and starts the server.
        /// </summary>
        public void Run()
        {
            if (_running)
                throw new Exception("Server is already running.");

            int x, y, width, height;
            Win32.GetWindowPosition(out x, out y, out width, out height);
            Win32.SetWindowPosition(0, 0, width, height);

            ConsoleUtil.WriteHeader($"Ranking Server ({Shared.Util.Version.GetVersion()})", ConsoleColor.DarkGreen);
            ConsoleUtil.LoadingTitle();

            Log.Info("Server startup requested");
            Log.Info($"Server Version {Shared.Util.Version.GetVersion()}");

            NavigateToRoot();

            // Conf
            LoadConf(Config = new RankingConf());

            // Database
            InitDatabase(Database = new RankingDatabase(), Config);

            // Start
            Server = new DefaultServer(Config.Ranking.Port);
            Server.Start();

            ConsoleUtil.RunningTitle();
            _running = true;

            // Commands
            var commands = new RankingConsoleCommands();
            commands.Wait();
        }
    }
}