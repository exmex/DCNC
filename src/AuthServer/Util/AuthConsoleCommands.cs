using System;
using System.Collections.Generic;
using Shared;
using Shared.Models;
using Shared.Objects;
using Shared.Util;
using Shared.Util.Commands;

namespace AuthServer.Util
{
    public class AuthConsoleCommands : ConsoleCommands
    {
        public AuthConsoleCommands()
        {
            Add("shutdown", "<seconds>", "Orders all servers to shut down", HandleShutDown);
            
            // Account commands
            Add("create", "<username> <password>", "Creates an account with the specified password",
                HandleAccountCreate);
            Add("passwd", "<username> <password>", "Changes password of account", HandlePasswd);
            Add("ban", "<username> (days)", "Bans the account", HandleBanAccount);
            Add("unban", "<username>", "Unbans the account", HandleUnbanAccount);
            Add("setperm", "<username> <permission>", "Set an accounts permission", HandleSetPerm);
        }

        private static CommandResult HandleSetPerm(string command, IList<string> args)
        {
            if(args.Count < 3)
                return CommandResult.InvalidArgument;
            var accountName = args[1];
            var strPerm = args[2];
            
            var user = AccountModel.Retrieve(AuthServer.Instance.Database.Connection, accountName);
            if (user == null)
            {
                Log.Error("Account not found!");
                return CommandResult.InvalidArgument;
            }
            
            UserPermission perm;
            if (!Enum.TryParse(args[2], true, out perm))
            {
                Log.Error("Invalid permission!");
                return CommandResult.InvalidArgument;
            }
            
            user.Permission = (int)perm;
            AccountModel.Update(AuthServer.Instance.Database.Connection, user);
            Log.Info("User {0} has now permission {1}!", user.Username, Enum.GetName(typeof(UserPermission), user.Permission));
            return CommandResult.Okay;
        }

        private static CommandResult HandleUnbanAccount(string command, IList<string> args)
        {
            if(args.Count < 2)
                return CommandResult.InvalidArgument;
            var accountName = args[1];

            var user = AccountModel.Retrieve(AuthServer.Instance.Database.Connection, accountName);
            if (user == null)
            {
                Log.Error("Account not found!");
                return CommandResult.InvalidArgument;
            }

            if (!user.IsUserBanned())
            {
                Log.Error("Account not banned!");
                return CommandResult.InvalidArgument;
            }

            if (user.Status != UserStatus.Banned)
            {
                Log.Error("Account not banned! (Old check)");
                return CommandResult.InvalidArgument;
            }
            
            user.Status = UserStatus.Normal;
            AccountModel.Update(AuthServer.Instance.Database.Connection, user);
            Log.Info("User {0} is not banned anymore!", user.Username);
            return CommandResult.Okay;
        }
        
        private static CommandResult HandleBanAccount(string command, IList<string> args)
        {
            if(args.Count < 2)
                return CommandResult.InvalidArgument;

            int days = 0;
            if (args.Count == 3)
            {
                if (!int.TryParse(args[2], out days) || days == 0)
                {
                    Log.Error("Invalid days specified!");
                    return CommandResult.InvalidArgument;
                }
            }
            
            var accountName = args[1];

            var user = AccountModel.Retrieve(AuthServer.Instance.Database.Connection, accountName);
            if (user == null)
            {
                Log.Error("Account not found!");
                return CommandResult.InvalidArgument;
            }

            if (user.IsUserBanned())
            {
                Log.Error("Account already banned!");
                return CommandResult.InvalidArgument;
            }

            if (user.Status != UserStatus.Normal)
            {
                Log.Error("Account already banned! (old check)");
                return CommandResult.InvalidArgument;
            }

            if (days != 0)
                user.BanValidUntil = DateTimeOffset.Now.AddDays(days).ToUnixTimeSeconds();
            else
                user.BanValidUntil = 0;
            
            user.Status = UserStatus.Banned;
            AccountModel.Update(AuthServer.Instance.Database.Connection, user);
            if(days != 0)
                Log.Info("User {0} is now banned until {1}!", user.Username, DateTimeOffset.FromUnixTimeSeconds(user.BanValidUntil));
            else
                Log.Info("User {0} is now banned!", user.Username);
            return CommandResult.Okay;
        }

        private static CommandResult HandleAccountCreate(string command, IList<string> args)
        {
            if (args.Count < 3)
                return CommandResult.InvalidArgument;

            var accountName = args[1];
            var password = args[2];

            AccountModel.CreateAccount(AuthServer.Instance.Database.Connection, "127.0.0.1", accountName, password);

            return CommandResult.Okay;
        }

        private static CommandResult HandleShutDown(string command, IList<string> args)
        {
            if (args.Count < 2)
                return CommandResult.InvalidArgument;

            // Get time
            int time;
            if (!int.TryParse(args[1], out time))
                return CommandResult.InvalidArgument;

            time = Math2.Clamp(60, 1800, time);

            // TODO: Broadcast shutdown to other servers!
            /*Send.ChannelShutdown(time);
            Log.Info("Shutdown request sent to all channel servers.");*/

            return CommandResult.Okay;
        }

        private static CommandResult HandlePasswd(string command, IList<string> args)
        {
            if (args.Count < 3)
                return CommandResult.InvalidArgument;

            var accountName = args[1];
            var password = args[2];
            
            var user = AccountModel.Retrieve(AuthServer.Instance.Database.Connection, accountName);
            if (user == null)
            {
                Log.Error("Account not found!");
                return CommandResult.Fail;
            }

            var salt = Password.CreateSalt(AccountModel.SaltSize);
            user.Password = Password.GenerateSaltedHash(password, salt);
            user.Salt = salt;
            AccountModel.Update(AuthServer.Instance.Database.Connection, user);

            Log.Info("Password change for {0} complete.", accountName);

            return CommandResult.Okay;
        }
    }
}