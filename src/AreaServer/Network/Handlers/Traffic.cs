using Shared.Network;
using Shared.Network.AreaServer;

namespace AreaServer.Network.Handlers
{
    internal class Traffic
    {
        [Packet(Packets.CmdUdpCastTcsSignal)]
        public static void UdpCastTcsSignal(Packet packet)
        {
            var udpCastTcsSignalPacket = new UdpCastTcsSignalPacket(packet);

            AreaServer.Instance.Server.Broadcast(new UdpCastTcsSignalAnswerPacket
            {
                Signal = udpCastTcsSignalPacket.Signal,
                Time = udpCastTcsSignalPacket.Time,
                State = udpCastTcsSignalPacket.State
            }.CreatePacket());
        }

        [Packet(Packets.CmdUdpCastTcs)]
        public static void UdpCastTcs(Packet packet)
        {
            // Traffic?
        }

        [Packet(Packets.CmdRegisterAgent)]
        public static void RegisterAgent(Packet packet)
        {
        }

        [Packet(Packets.CmdUdpCastTraffic)]
        public static void CastTraffic(Packet packet)
        {
        }
    }
}

/*
  unsigned __int16 m_TrafficCarId;
  unsigned __int16 m_Owner;
  unsigned __int16 m_carAttr;
  unsigned __int16 m_path;
  XiVec4 m_Pos;
  XiVec4 m_Vel;
  int m_OwnTime;
  int m_dwGlobalTime;
  int m_FreedTime; 

    &pArea->m_spaceGrid,
      lpMsg->m_x,
      lpMsg->m_y,
      320.0,
      (CastTask<BS_PktTCSSignal> *)&param);
*/