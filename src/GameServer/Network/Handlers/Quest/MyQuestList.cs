using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class MyQuestList
    {
        /*
        00 00 00 00  · · · · 
        * uint GivePostIdx
        */
        [Packet(Packets.CmdMyQuestList)]
        public static void Handle(Packet packet) // TODO: Send actual data
        {
            var quests = QuestModel.Retrieve(GameServer.Instance.Database.Connection, 0,
                packet.Sender.User.ActiveCharacterId);

            var ack = new MyQuestListAnswer
            {
                Quests = quests.ToArray()
            };
            packet.Sender.Send(ack.CreatePacket());
        }
    }
}