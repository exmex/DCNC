using Shared.Network;
using Shared.Network.AreaServer;

namespace AreaServer.Network.Handlers
{
    public class UdpCastTcsSignal
    {
        [Packet(Packets.CmdUdpCastTcsSignal)]
        public static void Handle(Packet packet)
        {
            var udpCastTcsSignalPacket = new UdpCastTcsSignalPacket(packet);

            AreaServer.Instance.Server.Broadcast(new UdpCastTcsSignalAnswerPacket
            {
                Signal = udpCastTcsSignalPacket.Signal,
                Time = udpCastTcsSignalPacket.Time,
                State = udpCastTcsSignalPacket.State
            }.CreatePacket());
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