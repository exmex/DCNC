namespace Shared.Network.AreaServer
{
    public class EnterAreaPacket
    {
        public readonly int AreaId;

        public readonly int GroupId;

        public readonly int LocalTime;
        public readonly short SessionId;

        public readonly string Username;

        public EnterAreaPacket(Packet packet)
        {
            SessionId = packet.Reader.ReadInt16();

            Username = packet.Reader.ReadUnicodeStatic(21);
            /*Username = packet.Reader.ReadUnicode();
            Username = Username.Substring(Username.Length - 1); // Strip trailing nullbyte
            */

            AreaId = packet.Reader.ReadInt32();
            LocalTime = packet.Reader.ReadInt32();
            GroupId = packet.Reader.ReadInt32();
        }
    }
}