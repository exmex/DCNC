using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Util;

namespace GameServer.Network.Handlers
{
    public class Messages
    {
        //PrivateChatMsg
        [Packet(Packets.CmdPrivateChatMsg)]
        public static void PrivateChatMsg(Packet packet)
        {
            // TODO: It's somehow missing the user?!
            var message = packet.Reader.ReadUnicodePrefixed();
            packet.Sender.SendError("User not found.");
            
            //GameServer.Instance.Server.GetClient();
        }
        
        [Packet(Packets.CmdChatMsg)]
        public static void ChatMessage(Packet packet)
        {
            var chatMsgPacket = new ChatMessagePacket(packet);

            var sender = packet.Sender.User.ActiveCharacter.Name;
            if (packet.Sender.User.GmFlag)
                sender = $"GM {sender}";

            if (packet.Sender.User.Status == UserStatus.Muted)
            {
                packet.Sender.SendError("You are currently blocked from chatting.");
                return;
            }

            Log.Debug($"({chatMsgPacket.MessageType}) <{sender}> {chatMsgPacket.Message}");

            var ack = new ChatMessageAnswer
            {
                MessageType = chatMsgPacket.MessageType,
                SenderCharacterName = sender,
                Message = chatMsgPacket.Message
            };

            var ackPkt = ack.CreatePacket();

            switch (chatMsgPacket.MessageType)
            {
                case "room":
                    GameServer.Instance.Server.Broadcast(ackPkt); // TODO: broadcast only to users in same area
                    break;

                case "channel":
                    GameServer.Instance.Server.Broadcast(ackPkt);
                    break;

                case "party":
                    if(packet.Sender.User.ActiveCharacter.PartyType == 65)
                        GameServer.Instance.Server.Broadcast(ackPkt); // TODO: broadcast only to users in same party
                    else
                        packet.Sender.SendError("You haven't joined any group.");
                    break;

                case "team":
                    if (packet.Sender.User.ActiveCharacter.Team != null)
                    {
                        GameServer.Instance.Server.BroadcastTeam(packet.Sender.User.ActiveCharacter.Team, ackPkt); // TODO: broadcast only to users in same crew
                    }
                    else
                        packet.Sender.SendError("Not a member of the crew.");
                    break;

                default:
                    packet.Sender.SendError("Cannot find the chat window.");
                    Log.Error("Undefined chat message type.");
                    break;
            }
        }
    }
}