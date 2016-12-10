using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Network;

namespace AreaServer.Network.Handlers
{
    class Traffic
    {

        [Packet(Packets.CmdUdpCastTcsSignal)]
        public static void UdpCastTcsSignal(Packet packet)
        {
            /*
              int m_AreaId;
              float m_x;
              float m_y;
              XiTCSSignal m_signal;

              int m_time;
              int m_signal;
              int m_state;
            */

            packet.Reader.ReadInt32(); // AreaId
            packet.Reader.ReadSingle(); // X
            packet.Reader.ReadSingle(); // Y

            packet.Reader.ReadInt32(); // Time
            packet.Reader.ReadInt32(); // Signal
            packet.Reader.ReadInt32(); // State
        }

        [Packet(Packets.CmdUdpCastTcs)]
        public static void UdpCastTcs(Packet packet)
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
*/
