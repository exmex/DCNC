namespace Shared.Network.GameServer
{
    public class QuestRewardPacket
    {
        public readonly uint TableIndex;

        public QuestRewardPacket(Packet packet)
        {
            TableIndex = packet.Reader.ReadUInt32();
        }
    }
}