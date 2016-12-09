using System;
using System.Windows.Forms;
using AuthServer.Database;
using AuthServer.Util;
using Shared;
using Shared.Network;
using Shared.Util;

namespace AuthServer
{
    public class AuthServer : ServerMain
    {
        public static readonly AuthServer Instance = new AuthServer();

        private bool _running;

        /// <summary>
        /// Instance of the actual server component.
        /// </summary>
        private DefaultServer Server { get; set; }

        /// <summary>
        /// Database
        /// </summary>
        public AuthDatabase Database { get; private set; }

        /// <summary>
        /// Configuration
        /// </summary>
        private AuthConf Config { get; set; }

        /// <summary>
        /// Initializes fields and properties
        /// </summary>
        private AuthServer()
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

            ConsoleUtil.WriteHeader("Auth Server", ConsoleColor.DarkGreen);
            ConsoleUtil.LoadingTitle();

            NavigateToRoot();

            // Conf
            LoadConf(Config = new AuthConf());

            // Database
            InitDatabase(Database = new AuthDatabase(), Config);

            // Start
            Server = new DefaultServer(Config.Auth.Port);
            Server.Start();

            ConsoleUtil.RunningTitle();
            _running = true;

            // Commands
            var commands = new AuthConsoleCommands();
            commands.Wait();
        }
    }
}
