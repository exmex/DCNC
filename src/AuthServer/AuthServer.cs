using System;
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
        ///     Initializes fields and properties
        /// </summary>
        private AuthServer()
        {
        }

        /// <summary>
        ///     Instance of the actual server component.
        /// </summary>
        private DefaultServer Server { get; set; }

        /// <summary>
        ///     Database
        /// </summary>
        public AuthDatabase Database { get; private set; }

        /// <summary>
        ///     Configuration
        /// </summary>
        public AuthConf Config { get; set; }

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

            ConsoleUtil.WriteHeader($"Auth Server ({Shared.Util.Version.GetVersion()})", ConsoleColor.DarkGreen);
            ConsoleUtil.LoadingTitle();

            Log.Info("Server startup requested");
            Log.Info($"Server Version {Shared.Util.Version.GetVersion()}");

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