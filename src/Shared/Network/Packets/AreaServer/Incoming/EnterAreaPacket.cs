namespace Shared.Network.AreaServer
{
    public class EnterAreaPacket
    {
        public readonly uint AreaId;

        public readonly uint GroupId;

        public readonly uint LocalTime;
        public readonly short SessionId;

        public readonly string Username;

        public EnterAreaPacket(Packet packet)
        {
            SessionId = packet.Reader.ReadInt16();

            Username = packet.Reader.ReadUnicode();
            Username = Username.Substring(Username.Length - 1); // Strip trailing nullbyte

            AreaId = packet.Reader.ReadUInt32();
            LocalTime = packet.Reader.ReadUInt32();
            GroupId = packet.Reader.ReadUInt32();
        }
    }
}