namespace Shared.Network.GameServer
{
    public class InstantStartPacket
    {
        public readonly uint TableIndex;

        public InstantStartPacket(Packet packet)
        {
            TableIndex = packet.Reader.ReadUInt32();
        }
    }
}