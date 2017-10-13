using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class MyTeamInfo
    {
        [Packet(Packets.CmdMyTeamInfo)]
        public static void Handle(Packet packet)
        {
            var myTeamInfoPacket = new MyTeamInfoPacket(packet);
            
            var user = packet.Sender.User;
            
            var ack = new MyTeamInfoAnswer
            {
                Action = myTeamInfoPacket.Action,
                CharacterId = user.ActiveCharacterId,
                Rank = 0,
                Crew = user.ActiveCharacter.Crew,
                Age = 0
            };

            packet.Sender.Send(ack.CreatePacket());
            /*
              unsigned int m_Act;
              __int64 m_Cid;
              int m_TeamRank;
              XiStrTeamInfo m_TeamInfo;
              unsigned __int16 m_Age;
            */
        }
    }
}