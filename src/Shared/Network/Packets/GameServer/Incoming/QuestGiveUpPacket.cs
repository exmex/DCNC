namespace Shared.Network.GameServer
{
    public class QuestGiveUpPacket
    {
        public readonly uint TableIndex;

        public QuestGiveUpPacket(Packet packet)
        {
            TableIndex = packet.Reader.ReadUInt32();
        }
    }
}