using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Shared.Util;

namespace Shared.Network.Packets.GameServer.BattleZone
{
    public class GetRoomInfoAnswer : OutPacket
    {

        public int m_Act;
        public uint m_RoomId;
        public uint m_Size;
        public uint m_unit;
        //BS_PktGetRoomInfoAck::Unit m_unit[1];

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packetss.CmdRoomList);
        }

        public override int ExpectedSize() => 10;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {

                    bs.Write(m_Act);
                    //bs.Write(m_RoomFilter);
                    bs.Write(m_RoomId);
                    bs.Write(m_Size);
                    bs.Write(m_unit);
                    

                }
                return ms.ToArray();
            }
        }
    }
}
