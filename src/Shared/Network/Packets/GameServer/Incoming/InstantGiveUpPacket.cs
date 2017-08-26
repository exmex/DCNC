namespace Shared.Network.GameServer
{
    public class InstantGiveUpPacket
    {
        public readonly uint TableIndex;

        public InstantGiveUpPacket(Packet packet)
        {
            TableIndex = packet.Reader.ReadUInt32();
        }
    }
}