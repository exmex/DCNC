using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class QuestComplete
    {
        //000000: 01 00 00 00 14 43 8B 42 4E 9E D0 FE  · · · · · C · B N · · ·
        [Packet(Packets.CmdQuestComplete)]
        public static void Handle(Packet packet)
        {
            var questCompletePacket = new QuestCompletePacket(packet);

            QuestModel.Update(GameServer.Instance.Database.Connection, 0, packet.Sender.User.ActiveCharacterId,
                questCompletePacket.TableIndex, 1);

            var ack = new QuestCompleteAnswer {TableIndex = questCompletePacket.TableIndex};
            packet.Sender.Send(ack.CreatePacket());
        }
    }
}