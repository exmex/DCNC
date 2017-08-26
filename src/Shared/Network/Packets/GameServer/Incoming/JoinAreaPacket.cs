namespace Shared.Network.GameServer
{
    public class JoinAreaPacket
    {
        public int AreaId;
        
        public JoinAreaPacket(Packet packet)
        {
            AreaId = packet.Reader.ReadInt32();
        }
    }
}