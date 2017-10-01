using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class QuestGiveUp
    {
        [Packet(Packets.CmdQuestGiveUp)]
        public static void Handle(Packet packet)
        {
            var questGiveUpPacket = new QuestGiveUpPacket(packet);

            QuestModel.Update(GameServer.Instance.Database.Connection, 0, packet.Sender.User.ActiveCharacterId,
                questGiveUpPacket.TableIndex, 4);

            var ack = new QuestGiveUpAnswer {TableIndex = questGiveUpPacket.TableIndex};

            packet.Sender.Send(ack.CreatePacket());
        }
    }
}