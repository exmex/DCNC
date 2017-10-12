using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class QuestStart
    {
        //000000: 01 00 00 00  · · · ·
        [Packet(Packets.CmdQuestStart)]
        public static void Handle(Packet packet)
        {
            var questStartPacket = new QuestStartPacket(packet);
			
			if(QuestModel.QuestStarted(GameServer.Instance.Database.Connection, 
				packet.Sender.User.ActiveCharacterId, (uint)questStartPacket.TableIndex))
			{
				packet.Sender.SendDebugError("Quest already started!");
				packet.Sender.KillConnection("Quest was already started!");
				return;
			}

            QuestModel.Add(GameServer.Instance.Database.Connection, new Quest
            {
                CharacterId = packet.Sender.User.ActiveCharacterId,
                CharacterName = packet.Sender.User.ActiveCharacter.Name,
                FailNum = 0,
                PlaceIdx = 0,
                QuestId = questStartPacket.TableIndex,
                ServerId = 0,
                State = 0
            });

            var ack = new QuestStartAnswer
            {
                TableIndex = questStartPacket.TableIndex,
                FailNum = 0
            };
            packet.Sender.Send(ack.CreatePacket());
        }
    }
}