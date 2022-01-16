using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Network.Packets.GameServer.BattleZone;
using Shared.Network;

namespace GameServer.Network.Handlers.BattleZone
{
    public class GetRoomInfo
    {
        [Packet(Packetss.CmdGetRoomInfo)]
        public static void Handle(Packet packet)
        {
            /*var RoomListPacket = new GetRoomInfoPacket(packet);
            packet.Sender.Send(new GetRoomInfoAnswer()
            {

                m_Act = packet.Reader.ReadInt32(),
                m_RoomId = packet.Reader.ReadUInt32(),
                

            }.CreatePacket());*/

            var m_Act = packet.Reader.ReadInt32();
            var m_RoomId = packet.Reader.ReadUInt32();

            var ack = new Packet(Packetss.GetRoomInfoAck);


            ack.Writer.Write(m_Act);
            ack.Writer.Write(m_RoomId);
            
            packet.Sender.Send(ack);

        }

        /*int {
        __unaligned __declspec(align(1)) int m_Act;
        __unaligned __declspec(align(1)) unsigned int m_RoomId;
        __unaligned __declspec(align(1)) unsigned int m_Size;
        BS_PktGetRoomInfoAck::Unit m_unit[1];
        };;*/
    }
}
