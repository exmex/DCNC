using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Objects;

namespace Shared.Network.Packets.GameServer.BattleZone
{
    public class RoomJoinPacket
    {
        /*{
        unsigned __int16 m_Serial;
        unsigned __int16 m_Age;
        __unaligned __declspec(align(1)) XiPvpUserInfo m_UserInfo;
        __unaligned __declspec(align(1)) XiCarAttr m_CarAttr;
        __unaligned __declspec(align(1)) unsigned int m_RoomId;
        __unaligned __declspec(align(1)) unsigned int m_RoomLifeId;
        __unaligned __declspec(align(1)) XiPlayerInfo m_PlayerInfo;
        XiPvpRoomSlot m_Slot;
        __int16 m_RoomType;*/
    

        public ushort m_Serial;
        public ushort m_Age;
        public XiPvpUserInfo m_UserInfo;
        public XiCarAttr m_CarAttr;
        public uint m_RoomId;
        public uint m_RoomLifeId;
        public XiPlayerInfo m_PlayerInfo;
        public XiPvpRoomSlot m_Slot;
        public ushort m_RoomType;

        /*public m_UserInfo()
        {
            Level = new Level();
            Port = new Port();
            Ip = new Ip();
            
        }*/

        public RoomJoinPacket(Packet packet)
        {
            m_Serial = packet.Reader.ReadUInt16();
            m_Age = packet.Reader.ReadUInt16();
            m_UserInfo = XiPvpUserInfo.Deserialize(packet.Reader);
            m_CarAttr = new XiCarAttr();
            m_RoomId = packet.Reader.ReadUInt32();
            m_RoomLifeId = packet.Reader.ReadUInt32();
            m_PlayerInfo = new XiPlayerInfo(packet.Sender.User.VehicleSerial, packet.Sender.User.ActiveCharacter);
            m_Slot = XiPvpRoomSlot.Deserialize(packet.Reader);
            m_RoomType = packet.Reader.ReadUInt16();

        }

    }
}
