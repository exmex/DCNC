namespace Shared.Network.GameServer
{
    public class QuestStartPacket
    {
        public readonly uint TableIndex;

        public QuestStartPacket(Packet packet)
        {
            TableIndex = packet.Reader.ReadUInt32();
        }
    }
}