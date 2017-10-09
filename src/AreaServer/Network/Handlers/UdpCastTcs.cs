using Shared.Network;

namespace AreaServer.Network.Handlers
{
    public static class UdpCastTcs
    {
        [Packet(Packets.CmdUdpCastTcs)]
        public static void Handle(Packet packet)
        {
            packet.Reader.ReadInt32();
            packet.Reader.ReadSingle();
            packet.Reader.ReadSingle();
            
            int m_time = packet.Reader.ReadInt32();
            float m_pos = packet.Reader.ReadSingle();
            float m_vel = packet.Reader.ReadSingle();
            float m_acl = packet.Reader.ReadSingle();
            ushort m_TrafficCarId = packet.Reader.ReadUInt16();
            ushort m_carAttr = packet.Reader.ReadUInt16();
            short m_path = packet.Reader.ReadInt16();
            short m_nextPath = packet.Reader.ReadInt16();
            short m_thirdPath = packet.Reader.ReadInt16();

            var ack = new Packet(Packets.UdpCastTcsAck);
            ack.Writer.Write(m_time);
            ack.Writer.Write(m_pos);
            ack.Writer.Write(m_vel);
            ack.Writer.Write(m_acl);
            ack.Writer.Write(m_TrafficCarId);
            ack.Writer.Write(m_carAttr);
            ack.Writer.Write(m_path);
            ack.Writer.Write(m_nextPath);
            ack.Writer.Write(m_thirdPath);
            //AreaServer.Instance.Server.Broadcast(ack);
            //packet.Sender.Send(ack);
            /*
              __unaligned __declspec(align(1)) int m_AreaId;
  __unaligned __declspec(align(1)) float m_x;
  __unaligned __declspec(align(1)) float m_y;
  XiTCSCarMove m_move;
            */
            //BS_PktMoveTCSVehicle
            /*
              int m_time;
              float m_pos;
              hfloat m_vel;
              hfloat m_acl;
              unsigned __int16 m_TrafficCarId;
              unsigned __int16 m_carAttr;
              __int16 m_path;
              __int16 m_nextPath;
              __int16 m_thirdPath;
            */
            // Traffic?
        }
    }
}