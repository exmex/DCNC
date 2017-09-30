using System.Collections.Generic;
using System.Linq;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Util.Commands;

namespace GameServer.Util
{
    public class GameChatCommands : ChatCommands
    {
        public GameChatCommands()
        {
            // As per legal requirements, this shall not be removed or changed!
            // As per legal requirements, this shall not be removed or changed!
            // As per legal requirements, this shall not be removed or changed!
            Add("copyright", "/copyright", 0x0, "Gets the copyright", CopyrightCommandHandler);
            Add("about", "/about", 0x0, "Gets the copyright", CopyrightCommandHandler);
            // As per legal requirements, this shall not be removed or changed!
            // As per legal requirements, this shall not be removed or changed!
            // As per legal requirements, this shall not be removed or changed!

            Add("help", "/help [Command]", 0x1000, "Shows help about a command", HelpCommandHandler);

            Add("notice", "/notice [Message]", 0x1000, "Send a GM notice", NoticeCommandHandler);
            Add("weather", "/weather [fine/cloudy/foggy/rain/sunset]", 0x8000, "Changes the current weather",
                WeatherCommandHandler);
            Add("kick", "/kick [Character Name]", 0x1000, "Kicks the user", KickCommandHandler);
            Add("ban", "/ban [Character Name]", 0x8000, "Bans the user forever", BanCommandHandler);
            //Add("tempban", "/tempban [Character Name]", 0x8000, "Bans the user", BanCommandHandler);
            Add("money", "/money [Character Name] [Amount]", 0x8000, "Gives the charactername money",
                MoneyCommandHandler);
            Add("exp", "/exp [Character Name] [Amount]", 0x8000, "Gives the user experience", ExpCommandHandler);

            Add("mute", "/mute [Character Name]", 0x8000, "Mutes/Unmutes the character from chat", MuteCommandHandler);
            Add("tempmute", "/mute [Character Name]", 0x8000, "Mutes/Unmutes the character from chat", MuteCommandHandler);

            Add("gm", "/gm", 0x1000, "Toggles your GM Status", ToggleGmStatusCommandHandler);
        }

        private static CommandResult MuteCommandHandler(DefaultServer server, Client sender, string command,
            IList<string> args)
        {
            if (args.Count < 1)
                return CommandResult.InvalidArgument;

            var characterName = args[0];

            var client = GameServer.Instance.Server.GetClient(characterName);
            if (client?.User == null) return CommandResult.Fail;
            if(client.User.Status == UserStatus.Banned) return CommandResult.Fail;

            client.User.Status = client.User.Status == UserStatus.Muted ? UserStatus.Normal : UserStatus.Muted;
            if(command == "mute") // temp mute doesn't save to the db!
                AccountModel.Update(GameServer.Instance.Database.Connection, client.User);

            var newStatusStr = client.User.Status == UserStatus.Muted ? "muted" : "unmuted";
            sender.SendChatMessage($"User {client.User.Username} was {newStatusStr}");

            if (sender.User.GmFlag)
                sender.SendChatMessage($"You were {newStatusStr} by {sender.User.Username}");
            else
                sender.SendChatMessage($"You were {newStatusStr} by a GM");

            return CommandResult.Okay;
        }

        private static CommandResult ToggleGmStatusCommandHandler(DefaultServer server, Client sender, string command,
            IList<string> args)
        {
            sender.User.GmFlag = !sender.User.GmFlag;
            var status = "invisible";
            if (sender.User.GmFlag)
                status = "visible";

            sender.SendChatMessage($"Your GM Status is now: {status}");
            return CommandResult.Okay;
        }

        private static CommandResult HelpCommandHandler(DefaultServer server, Client sender, string command,
            IList<string> args)
        {
            if (args.Count < 1)
                return CommandResult.InvalidArgument;
            var cmd = args[0];
            var helpCmd = GameServer.ChatCommands.GetCommand(cmd);
            if(helpCmd == null)
                return CommandResult.Fail;
            
            if (helpCmd.RequiredPermission > sender.User.Permission)
                return CommandResult.Fail;

            sender.SendChatMessage(helpCmd.Usage + " - " + helpCmd.Description);

            return CommandResult.Okay;
        }

        private static CommandResult CopyrightCommandHandler(DefaultServer server, Client sender, string command,
            IList<string> args)
        {
            // As per legal requirements, this shall not be removed or changed!
            // As per legal requirements, this shall not be removed or changed!
            // As per legal requirements, this shall not be removed or changed!

            sender.SendChatMessage($"Drift City Neo City v{Shared.Util.Version.GetVersion()} Copyright 2016 GigaToni");
            return CommandResult.Okay;

            // As per legal requirements, this shall not be removed or changed!
            // As per legal requirements, this shall not be removed or changed!
            // As per legal requirements, this shall not be removed or changed!
        }

        private static CommandResult NoticeCommandHandler(DefaultServer server, Client sender, string command,
            IList<string> args)
        {
            // Doesn't work right. Sending /notice notice does work. But "prints out notice XYZ" to chat..
            if (args.Count == 0)
                return CommandResult.InvalidArgument;

            var msg = string.Join(" ", args);
            var ack = new ChatMessageAnswer()
            {
                MessageType = "channel",
                SenderCharacterName = "GM",
                Message = msg,
            }.CreatePacket();

            server.Broadcast(ack);

            return CommandResult.Okay;
        }

        private static CommandResult WeatherCommandHandler(DefaultServer server, Client sender, string command,
            IList<string> args)
        {
            if (args.Count < 1)
                return CommandResult.InvalidArgument;

            var ack = new Packet(Packets.WeatherAck);
            switch (args[0])
            {
                case "fine":
                    ack.Writer.Write(0);
                    break;
                case "cloudy":
                    ack.Writer.Write(1);
                    break;
                case "foggy":
                    ack.Writer.Write(2);
                    break;
                case "rain":
                    ack.Writer.Write(3);
                    break;
                case "sunset":
                    ack.Writer.Write(4);
                    break;
                default:
                    return CommandResult.InvalidArgument;
            }

            GameServer.Instance.Server.Broadcast(ack);

            return CommandResult.Okay;
        }

        private static CommandResult KickCommandHandler(DefaultServer server, Client sender, string command,
            IList<string> args)
        {
            if (args.Count < 1)
                return CommandResult.InvalidArgument;

            var characterName = args[0];

            var client = GameServer.Instance.Server.GetClient(characterName);
            if (client?.User == null) return CommandResult.Fail;

            client.KillConnection($"Kicked by {sender.User.Username}");
            sender.SendChatMessage($"User {characterName} ({client.User.Username}) kicked!");

            return CommandResult.Okay;
        }

        private static CommandResult MoneyCommandHandler(DefaultServer server, Client sender, string command,
            IList<string> args)
        {
            //const char *CName, __int64 Money, const char *szEventCode
            if (args.Count < 2)
                return CommandResult.InvalidArgument;

            var characterName = args[0];
            long amount;
            if (!long.TryParse(args[1], out amount)) return CommandResult.InvalidArgument;
            if (amount <= 0) // Allow only positive numbers
                return CommandResult.InvalidArgument;

            var client = GameServer.Instance.Server.GetClient(characterName);
            // Client / Character is not online! (This activeChar check is redundant, see GetClient(characterName)
            if (client?.User.ActiveCharacter == null) return CommandResult.Fail;

            client.User.ActiveCharacter.MitoMoney += amount;
            CharacterModel.Update(GameServer.Instance.Database.Connection, client.User.ActiveCharacter);

            sender.SendChatMessage($"{amount} Mito given to {characterName} ({client.User.Username})");

            client.Send(new CharUpdateAnswer()
            {
                Character = client.User.ActiveCharacter
            }.CreatePacket());

            return CommandResult.Okay;
        }

        private static CommandResult ExpCommandHandler(DefaultServer server, Client sender, string command,
            IList<string> args)
        {
            if (args.Count < 2)
                return CommandResult.InvalidArgument;

            var characterName = args[0];
            int amount;
            if (!int.TryParse(args[1], out amount)) return CommandResult.InvalidArgument;
            if (amount <= 0) // Allow only positive numbers
                return CommandResult.InvalidArgument;

            var client = GameServer.Instance.Server.GetClient(characterName);

            // Client / Character is not online! (This activeChar check is redundant, see GetClient(characterName)
            if (client?.User.ActiveCharacter == null) return CommandResult.Fail;

            bool levelUp;
            bool useBonus = false;
            bool useBonus500Mita = false;
            client.User.ActiveCharacter.CalculateExp(amount, out levelUp, useBonus, useBonus500Mita);
            // TODO: Check if user has leveled up, if so send levelup packet!

            CharacterModel.Update(GameServer.Instance.Database.Connection, client.User.ActiveCharacter);

            sender.SendChatMessage($"{amount} EXP given to {characterName} ({client.User.Username})");

            client.Send(new CharUpdateAnswer()
            {
                Character = client.User.ActiveCharacter
            }.CreatePacket());

            return CommandResult.Okay;
        }

        private static CommandResult BanCommandHandler(DefaultServer server, Client sender, string command,
            IList<string> args)
        {
            if (args.Count < 1)
                return CommandResult.InvalidArgument;

            var characterName = args[0];

            var client = GameServer.Instance.Server.GetClient(characterName);
            if (client?.User == null) return CommandResult.Fail;

            client.User.Status = UserStatus.Banned;
            AccountModel.Update(GameServer.Instance.Database.Connection, client.User);

            client.KillConnection($"Banned by {sender.User.Username}");
            sender.SendChatMessage($"User {characterName} ({client.User.Username}) banned!");

            return CommandResult.Okay;
        }
    }
}