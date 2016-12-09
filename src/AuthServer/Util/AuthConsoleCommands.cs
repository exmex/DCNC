using System.Collections.Generic;
using Shared;
using Shared.Models;
using Shared.Util;
using Shared.Util.Commands;

namespace AuthServer.Util
{
    public class AuthConsoleCommands : ConsoleCommands
    {
        public AuthConsoleCommands()
        {
            Add("shutdown", "<seconds>", "Orders all servers to shut down", HandleShutDown);
            Add("passwd", "<username> <password>", "Changes password of account", HandlePasswd);
            Add("create", "<username> <password>", "Creates an account with the specified password", HandleAccountCreate);
        }

        private CommandResult HandleAccountCreate(string command, IList<string> args)
        {
            if (args.Count < 3)
                return CommandResult.InvalidArgument;

            var accountName = args[1];
            var password = args[2];

            AccountModel.CreateAccount(AuthServer.Instance.Database.Connection, "127.0.0.1", accountName, password);

            return CommandResult.Okay;
        }

        private CommandResult HandleShutDown(string command, IList<string> args)
        {
            if (args.Count < 2)
                return CommandResult.InvalidArgument;

            // Get time
            int time;
            if (!int.TryParse(args[1], out time))
                return CommandResult.InvalidArgument;

            time = Math2.Clamp(60, 1800, time);

            /*Send.ChannelShutdown(time);
            Log.Info("Shutdown request sent to all channel servers.");*/

            return CommandResult.Okay;
        }

        private CommandResult HandlePasswd(string command, IList<string> args)
        {
            if (args.Count < 3)
            {
                return CommandResult.InvalidArgument;
            }

            var accountName = args[1];
            var password = args[2];

            if (!AccountModel.AccountExists(AuthServer.Instance.Database.Connection, accountName))
            {
                Log.Error("Please specify an existing account.");
                return CommandResult.Fail;
            }

            AccountModel.SetAccountPassword(AuthServer.Instance.Database.Connection, accountName, password);

            Log.Info("Password change for {0} complete.", accountName);

            return CommandResult.Okay;
        }
    }
}
