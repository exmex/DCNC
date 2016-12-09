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
        /// Instance of the actual server component.
        /// </summary>
        private DefaultServer Server { get; set; }

        /// <summary>
        /// Database
        /// </summary>
        public RankingDatabase Database { get; private set; }

        /// <summary>
        /// Configuration
        /// </summary>
        private RankingConf Config { get; set; }

        /// <summary>
        /// Initializes fields and properties
        /// </summary>
        private RankingServer()
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
            SetWindowPosition(0, 0, width, height);

            ConsoleUtil.WriteHeader("Ranking Server", ConsoleColor.DarkGreen);
            ConsoleUtil.LoadingTitle();

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
