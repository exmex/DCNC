using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Network;
using Shared.Network.Packets.GameServer.BattleZone;
using Shared.Objects;

namespace GameServer.Network.Handlers
{
    public class RoomDirectJoin
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


        [Packet(Packetss.CmdRoomDirectJoin)]
        public static void Handle(Packet packet)
        {
            //var m_nPvpChannelId = packet.Reader.ReadString();
            //var m_Serial = 1;
            //var m_UserInfo = 1;
            //var m_PlayerInfo = 1;
            //var m_CarAttr = packet.Reader.ReadInt16();
            //var m_RoomFilter = packet.Reader.ReadUInt16();*/
            
            var CmdRoomDirectJoinPacket = new RoomDirectJoinPacket(packet);
            packet.Sender.Send(new RoomDirectJoinAnswer()
            {
                m_nPvpChannelId = packet.Reader.ReadInt32(),
                m_Serial = packet.Reader.ReadUInt32(),
                m_UserInfo = XiPvpUserInfo.Deserialize(packet.Reader),
                m_PlayerInfo = new XiPlayerInfo(packet.Sender.User.VehicleSerial, packet.Sender.User.ActiveCharacter),
                m_CarAttr = new XiCarAttr(),
                m_RoomFilter = (XiPvpRoomFilter)packet.Reader.ReadInt32(),
        }.CreatePacket());

        }
    }
}
