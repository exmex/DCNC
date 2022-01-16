using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Network.Packets.GameServer.BattleZone;
using Shared.Objects;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Models;

namespace GameServer.Network.Handlers.BattleZone
{
    public class RoomJoin
    {
        [Packet(Packetss.CmdRoomJoin)]
        public static void Handle(Packet packet)
        {
            var RoomJoinPacket = new RoomJoinPacket(packet);
            packet.Sender.Send(new RoomJoinAnswer()
            {
                m_Serial = packet.Reader.ReadUInt16(),
                m_Age = packet.Reader.ReadUInt16(),
                m_UserInfo = new XiPvpUserInfo(),
                m_CarAttr = new XiCarAttr(),
                m_RoomId = packet.Reader.ReadUInt32(),
                m_RoomLifeId = packet.Reader.ReadUInt32(),
                m_PlayerInfo = new XiPlayerInfo(),
                m_Slot = new XiPvpRoomSlot(),
                m_RoomType = packet.Reader.ReadUInt16(),
        
            }.CreatePacket());

            /* unsigned __int16 m_Serial;
  unsigned __int16 m_Age;
  __unaligned __declspec(align(1)) XiPvpUserInfo m_UserInfo;
  __unaligned __declspec(align(1)) XiCarAttr m_CarAttr;
  __unaligned __declspec(align(1)) unsigned int m_RoomId;
  __unaligned __declspec(align(1)) unsigned int m_RoomLifeId;
  __unaligned __declspec(align(1)) XiPlayerInfo m_PlayerInfo;
  XiPvpRoomSlot m_Slot;
  __int16 m_RoomType;*/
        }

    }
}
