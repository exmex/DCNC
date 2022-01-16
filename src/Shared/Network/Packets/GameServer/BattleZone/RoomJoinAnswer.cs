using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Shared.Util;
using Shared.Network.Packets.GameServer;
using Shared.Objects;

namespace Shared.Network.Packets.GameServer.BattleZone
{
    public class RoomJoinAnswer : OutPacket
    {
        public ushort m_Serial;
        public ushort m_Age;
        public XiPvpUserInfo m_UserInfo;
        public XiCarAttr m_CarAttr;
        public uint m_RoomId;
        public uint m_RoomLifeId;
        public XiPlayerInfo m_PlayerInfo;
        public XiPvpRoomSlot m_Slot;
        public ushort m_RoomType;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packetss.CmdRegisterRoomObserver);
        }

        public override int ExpectedSize() => 10;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(m_Serial);
                    bs.Write(m_Age);
                    bs.Write(m_UserInfo);
                    bs.Write((BinaryWriterExt.ISerializable)m_CarAttr);
                    bs.Write(m_RoomId);
                    bs.Write(m_RoomLifeId);
                    bs.Write(m_PlayerInfo);
                    bs.Write(m_Slot);
                    bs.Write(m_RoomType);
                }
                return ms.ToArray();
            }
        }

    }
}
