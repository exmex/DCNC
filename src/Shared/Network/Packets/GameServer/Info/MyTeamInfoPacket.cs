namespace Shared.Network.GameServer
{
    public class MyTeamInfoPacket
    {
        // Action (1003, 1004, 1031, 1034)
        public readonly uint Action;
        
        public MyTeamInfoPacket(Packet packet)
        {
            Action = packet.Reader.ReadUInt32(); // nAct?
        }
    }
}