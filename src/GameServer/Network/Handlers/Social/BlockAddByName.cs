using Shared.Models;
using Shared.Network;

namespace GameServer.Network.Handlers.Social
{
    public class BlockAddByName
    {
        [Packet(Packets.CmdBlockAddByName)]
        public static void Handle(Packet packet)
        {
            /*
            [Debug] - 226: 000000: 31 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  1 · · · · · · · · · · · · · · ·
            000016: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · ·
            000032: 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · ·

            Wrong Packet Size. CMD(227) CmdLen: : 114, AnalysisSize: 4
            */

            var charName = packet.Reader.ReadUnicodeStatic(21);

            if (FriendModel.AddByName(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId,
                charName, 'B'))
                FriendList.Handle(packet);
        }
    }
}
/*
         * 569 -> BlockList?
        struct XiBlockInfo
        {
          unsigned int m_Serial;
          int m_DueTime;
        }; 
        */