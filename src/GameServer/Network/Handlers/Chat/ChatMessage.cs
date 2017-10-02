using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Util;

namespace GameServer.Network.Handlers
{
    public class ChatMessage
    {
        [Packet(Packets.CmdChatMsg)]
        public static void Handle(Packet packet)
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
                    GameServer.Instance.Server.BroadcastArea(0, ackPkt);
                    break;

                case "channel":
                    if(packet.Sender?.User?.ActiveCharacter != null)
                        GameServer.Instance.Server.BroadcastChannel(packet.Sender.User.ActiveCharacter.LastChannel, ackPkt);
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
                        GameServer.Instance.Server.Broadcast(packet.Sender.User.ActiveCharacter.Team, ackPkt); // TODO: broadcast only to users in same crew
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