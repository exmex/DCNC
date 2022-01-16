using Shared;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Util;
using Shared.Network.Packets.GameServer.BattleZone;
using Shared.Objects;


namespace GameServer.Network.Handlers.BattleZone
{
    public class RegisterRoomObserver
    {
        [Packet(Packetss.CmdRegisterRoomObserver)]
        public static void Handle(Packet packet)
        {
            
            var registerRoomObserverPacket = new RegisterRoomObserverPacket(packet);

            var m_Result = 1;
            var m_PvpChannelId = registerRoomObserverPacket.m_PvpChannelId; //packet.Reader.ReadInt32();
            var m_RoomFilter = registerRoomObserverPacket.m_RoomFilter;
            var m_Page = registerRoomObserverPacket.m_Page;
            var m_RealMatchEnable = 0; //TODO send from game settings
            var m_RealMatchTime = 0; //TODO send from game settings

            var ack = new Packet(Packetss.RegisterRoomObserverAck);


            ack.Writer.Write(m_Result);
            ack.Writer.Write(m_PvpChannelId);
            ack.Writer.Write((byte)m_RoomFilter);
            //ack.Writer.Write((int)new XiPvpRoomFilter());
            ack.Writer.Write(m_Page);
            ack.Writer.Write(m_RealMatchEnable);
            ack.Writer.Write(m_RealMatchTime);


            packet.Sender.Send(ack);
        }
    }
}
