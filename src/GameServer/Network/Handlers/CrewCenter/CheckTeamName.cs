using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;

namespace GameServer.Network.Handlers
{
    public static class CheckTeamName
    {
        [Packet(Packets.CmdCheckTeamName)]
        public static void Handle(Packet packet)
        {
            var checkTeamNamePacket = new CheckTeamNamePacket(packet);
                
            var nameTaken = TeamModel.CheckNameExists(GameServer.Instance.Database.Connection,
                checkTeamNamePacket.TeamName);
            
            packet.Sender.Send(new CheckTeamNameAnswer()
            {
                Availability = !nameTaken,
                TeamName = checkTeamNamePacket.TeamName
            }.CreatePacket());
        }
    }
}