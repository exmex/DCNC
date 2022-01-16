using Shared.Objects;

namespace Shared.Network.GameServer
{
    public class RoomCreatePacket
    {
        //public readonly string m_UserInfo;
        public readonly XiPvpUserInfo m_UserInfo;
        public readonly short m_RoomType;
        public readonly ushort m_MapId;
        public readonly ushort m_PlayerCapacity;
        public readonly ushort m_MapFlag;
        public readonly string m_RoomName; // 30 char
        public readonly string m_RoomPass; // 30 char

        
        public RoomCreatePacket(Packet packet)
        {
            m_UserInfo = XiPvpUserInfo.Deserialize(packet.Reader);
            //m_UserInfo = new XiPvpUserInfo();
            m_RoomType = packet.Reader.ReadInt16(); //0 = individual 1 = Team 2 = Practice
            m_MapId = packet.Reader.ReadUInt16();
            m_PlayerCapacity = packet.Reader.ReadUInt16();
            m_MapFlag = packet.Reader.ReadUInt16(); //Spectate??
            m_RoomName = packet.Reader.ReadUnicodeStatic(15);  //Room Name
            m_RoomPass = packet.Reader.ReadUnicodeStatic(15); // Password
        }
    }
}