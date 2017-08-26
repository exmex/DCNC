namespace Shared.Network.GameServer
{
    public class ChatMessagePacket
    {
        public readonly string MessageType;
        public readonly bool IsGreen; // ignore this, use packet.Sender.Player.User.Status
        public readonly string Message;
        
        public ChatMessagePacket(Packet packet)
        {
            MessageType = packet.Reader.ReadUnicodeStatic(10);
            IsGreen = packet.Reader.ReadUInt32() == 0xFF00FF00;
            Message = packet.Reader.ReadUnicodePrefixed();
        }
    }
}