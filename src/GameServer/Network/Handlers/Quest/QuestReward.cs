using Shared;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class QuestReward
    {
        [Packet(Packets.CmdQuestReward)]
        public static void Handle(Packet packet)
        {
            var questRewardPacket = new QuestRewardPacket(packet);

            var quest = QuestModel.RetrieveOne(GameServer.Instance.Database.Connection, 0,
                packet.Sender.User.ActiveCharacterId,
                questRewardPacket.TableIndex);
            QuestModel.Update(GameServer.Instance.Database.Connection, 0, packet.Sender.User.ActiveCharacterId,
                questRewardPacket.TableIndex, 2);
            if (quest == null)
            {
                packet.Sender.SendError("Quest was not started!");
                return;
            }
            if (quest.State != 1)
            {
                packet.Sender.SendError("Quest not finished!");
                return;
            }

            var questReward =
                ServerMain.Quests.Find(quest1 => quest1.TableIndex == questRewardPacket.TableIndex);
            if (questReward == null)
            {
                packet.Sender.SendError("Quest reward not found.");
                return;
            }
            var itemReward = questReward.GetRewards();
            var character = packet.Sender.User.ActiveCharacter;

            bool levelUp;
            const bool useBonus = false; // TODO: Determine boni
            const bool useBonus500Mita = false;
            character.CalculateExp(questReward.Experience, out levelUp, useBonus,
                useBonus500Mita);
				
			character.Mito += questReward.Mito;

            CharacterModel.Update(GameServer.Instance.Database.Connection, character);
            
            var item01 = 0;
            var item02 = 0;
            var item03 = 0;
            if (itemReward.Length > 0)
            {
                if (itemReward.Length >= 1)
                {
                    item01 = ServerMain.Items.FindIndex(item => item.Id == itemReward[0]);
                    if (item01 == -1 || character.GiveItem(GameServer.Instance.Database.Connection, item01, 1) ==
                        null)
                        item01 = 0;
                }
                if (itemReward.Length >= 2)
                {
                    item02 = ServerMain.Items.FindIndex(item => item.Id == itemReward[1]);
                    if (item01 == -1 || character.GiveItem(GameServer.Instance.Database.Connection, item02, 1) ==
                        null)
                        item02 = 0;
                }
                if (itemReward.Length == 3)
                {
                    item03 = ServerMain.Items.FindIndex(item => item.Id == itemReward[2]);
                    if (item01 == -1 || character.GiveItem(GameServer.Instance.Database.Connection, item03, 1) ==
                        null)
                        item03 = 0;
                }
            }
            
            var ack = new QuestRewardAnswer
            {
                TableIndex = (uint) questReward.TableIndex,
                GetExp = (uint) questReward.Experience,
                GetMoney = (uint) questReward.Mito,
                CurrentExp = (ulong) character.ExperienceInfo.CurExp,
                NextExp = (ulong) character.ExperienceInfo.NextExp,
                BaseExp = (ulong) character.ExperienceInfo.BaseExp,
                Level = character.Level,
                ItemNum = (ushort) itemReward.Length,
                RewardItem1 = (uint)item01,
                RewardItem2 = (uint)item02,
                RewardItem3 = (uint)item03
            };
            packet.Sender.Send(ack.CreatePacket());
            
            if(item01 != 0 || item02 != 0 || item03 != 0)
                character.FlushItemModBuffer(packet.Sender);
        }
    }
}