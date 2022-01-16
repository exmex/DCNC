using Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.Packets.GameServer.BattleZone
{
    public class RoomDirectJoinPacket
    {
        /*struct __cppobj __unaligned __declspec(align(2)) BS_PktRoomDirectJoin : BS_PktBody
        {
        int m_nPvpChannelId;
        unsigned int m_Serial;
        XiPvpUserInfo m_UserInfo;
        XiPlayerInfo m_PlayerInfo;
        XiCarAttr m_CarAttr;
        XiPvpRoomFilter m_RoomFilter;
    };*/
        public int m_nPvpChannelId;
        public uint m_Serial;
        public XiPvpUserInfo m_UserInfo;
        public XiPlayerInfo m_PlayerInfo;
        public XiCarAttr m_CarAttr;
        public ushort m_carAttr;
        public XiPvpRoomFilter m_RoomFilter;

        public RoomDirectJoinPacket(Packet packet)
        {

            m_nPvpChannelId = packet.Reader.ReadInt32();
            m_Serial = packet.Reader.ReadUInt32();
            m_UserInfo = XiPvpUserInfo.Deserialize(packet.Reader);
            m_PlayerInfo = new XiPlayerInfo(packet.Sender.User.VehicleSerial, packet.Sender.User.ActiveCharacter);
            m_CarAttr = new XiCarAttr();
            //m_carAttr = packet.Reader.ReadUInt16();
            m_RoomFilter = (XiPvpRoomFilter)packet.Reader.ReadInt32();

        }

    }
}
