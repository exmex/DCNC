using System.IO;
using Shared.Network.AreaServer;
using Shared.Util;
using Shared.Objects;

namespace Shared.Network.Packets.GameServer
{
    /// <summary>
    /// sub_685720
    /// </summary>
    public class RoomCreateAnswer : OutPacket
    {
        /// <summary>
        /// Arena Type
        /// Referenced as BS_Session
        /// Types:
        /// - lpSession = First Arena - upto V3
        /// - v5 = First Arena - upto V6
        /// - v7 = First Arena - upto V8
        /// - v9 = First Arena - upto V10
        /// - v11 = First Arena - upto V12
        /// - debug - Debug Messages
        /// </summary>


        /// <summary>
        /// Max Car Class
        /// Referenced as wchar_t
        /// /// Types:
        /// - lpSession = First Arena - upto V3
        /// - v5 = First Arena - upto V6
        /// - v7 = First Arena - upto V8
        /// - v9 = First Arena - upto V10
        /// - v11 = First Arena - upto V12
        /// - debug - Debug Messages
        /// </summary>


        /// <summary>
        /// The actual message
        /// ???
        /// </summary>
        //public XiPvpUserInfo m_UserInfo;

        /*__unaligned __declspec(align(1)) XiPvpUserInfo m_UserInfo;
            __unaligned __declspec(align(1)) int m_Result;
            __unaligned __declspec(align(1)) unsigned int m_RoomId;
            __unaligned __declspec(align(1)) unsigned int m_RoomLifeId;
            __int16 m_RoomType;
            unsigned __int16 m_MapId;
            unsigned __int16 m_MapFlag;*/
        public string m_UserInfo;
        public int m_Result;
        public uint m_RoomId;
        public uint m_RoomLifeId;
        public short m_RoomType;
        public ushort m_MapId;
        public ushort m_MapFlag;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packetss.CmdRoomCreate);
        }

        public override int ExpectedSize() => 10;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(m_UserInfo);
                    bs.Write(m_Result);
                    bs.Write(m_RoomId);
                    bs.Write(m_RoomLifeId);
                    bs.Write(m_RoomType);
                    bs.Write(m_MapId);
                    bs.Write(m_MapFlag);
                }
                return ms.ToArray();
            }
        }

    }

}
