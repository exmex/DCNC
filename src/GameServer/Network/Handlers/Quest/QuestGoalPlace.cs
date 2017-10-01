using Shared.Network;
using Shared.Util;

namespace GameServer.Network.Handlers
{
    public class QuestGoalPlace
    {
        //000000: 01 00 00 00  · · · ·
        //[Packet(Packets.CmdQuestGoalPlace)]
        public static void Handle(Packet packet)
        {
            Log.Unimplemented("Not implemented");
        }
    }
}