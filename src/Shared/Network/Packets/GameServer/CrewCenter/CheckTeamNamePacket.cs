namespace Shared.Network.GameServer
{
    public class CheckTeamNamePacket
    {
        public string TeamName;
        
        public CheckTeamNamePacket(Packet packet)
        {
            TeamName = packet.Reader.ReadUnicodeStatic(12);
        }
    }
}