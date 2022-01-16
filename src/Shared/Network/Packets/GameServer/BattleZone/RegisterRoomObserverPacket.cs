using Shared.Objects;

namespace Shared.Network.Packets.GameServer.BattleZone
{
    public class RegisterRoomObserverPacket
    {

        /// <summary>
        ///     The used protocol version on client side
        /// </summary>
        public readonly uint ProtocolVersion;

        public readonly uint m_PvpChannelId;
        public readonly XiPvpRoomFilter m_RoomFilter;
        //public readonly int m_RoomFilter;
        public readonly uint m_Page;
        public readonly uint m_PageSize;

        public RegisterRoomObserverPacket(Packet packet)
        {
            m_PvpChannelId = packet.Reader.ReadUInt32();
            m_RoomFilter = (XiPvpRoomFilter)packet.Reader.ReadInt32();
            //m_RoomFilter = packet.Reader.ReadInt32();
            m_Page = packet.Reader.ReadUInt32();
            m_PageSize = packet.Reader.ReadUInt32();
        }
    }
}

