namespace Shared.Network.GameServer
{
    public class InstantGoalPlacePacket
    {
        public readonly uint PlaceIndex;
        public readonly uint TableIndex;

        public InstantGoalPlacePacket(Packet packet)
        {
            TableIndex = packet.Reader.ReadUInt32();
            PlaceIndex = packet.Reader.ReadUInt32();
        }
    }
}