using System.Collections.Generic;
using System.Linq;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Util.Commands;

namespace GameServer.Util
{
    public class GameChatCommands : ChatCommands
    {
        public GameChatCommands()
        {
            Add("notice", "/notice [Message]", 0x1000, "Send a GM notice", (server, sender, command, args) =>
            {
                // Doesn't work right. Sending /notice notice does work. But "prints out notice XYZ" to chat..
                if(args.Count == 0)
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
            });
            
            Add("weather", "/weather [fine/cloudy/foggy/rain/sunset]", 0x8000, "Changes the current weather", (server, sender, command, args) =>
            {
                if(args.Count < 1)
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
            });
            
            Add("money", "/money [Character Name] [Amount]", 0x8000, "Gives the charactername money", (server, sender, command, args) =>
            {
                //const char *CName, __int64 Money, const char *szEventCode
                if(args.Count < 2)
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
                
                sender.Send(new ChatMessageAnswer()
                {
                    MessageType = "channel",
                    SenderCharacterName = "Server",
                    Message = $"{amount} Mito given to {characterName}",
                }.CreatePacket());
                
                client.Send(new CharUpdateAnswer()
                {
                    character = client.User.ActiveCharacter
                }.CreatePacket());
                
                return CommandResult.Okay;
            });
            
            Add("exp", "/exp [Character Name] [Amount]", 0x8000, "Gives the user experience", (server, sender, command, args) =>
            {
                if(args.Count < 2)
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
                    
                sender.Send(new ChatMessageAnswer()
                {
                    MessageType = "channel",
                    SenderCharacterName = "Server",
                    Message = $"{amount} EXP given to {characterName}",
                }.CreatePacket());
                
                client.Send(new CharUpdateAnswer()
                {
                    character = client.User.ActiveCharacter
                }.CreatePacket());
                
                return CommandResult.Okay;
            });
        }
    }
}