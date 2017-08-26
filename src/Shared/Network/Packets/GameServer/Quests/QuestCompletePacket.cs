namespace Shared.Network.GameServer
{
    public class QuestCompletePacket
    {
        public readonly uint TableIndex;

        public QuestCompletePacket(Packet packet)
        {
            TableIndex = packet.Reader.ReadUInt32();
        }
    }
}