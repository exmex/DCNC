using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Shared.Util;
using Shared.Objects;

namespace Shared.Network.Packets.GameServer.BattleZone
{
    public class RoomDirectJoinAnswer : OutPacket
    {
        public int m_nPvpChannelId;
        public uint m_Serial;
        public XiPvpUserInfo m_UserInfo;
        public XiPlayerInfo m_PlayerInfo;
        public XiCarAttr m_CarAttr;
        public XiPvpRoomFilter m_RoomFilter;

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

                    bs.Write(m_nPvpChannelId);
                    bs.Write(m_Serial);
                    bs.Write(m_UserInfo);
                    bs.Write(m_PlayerInfo);
                    bs.Write((BinaryWriterExt.ISerializable)m_CarAttr);
                    bs.Write((int)m_RoomFilter);

                }
                return ms.ToArray();
            }
        }

    }
}
