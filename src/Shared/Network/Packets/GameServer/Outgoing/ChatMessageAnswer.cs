using System.IO;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class ChatMessageAnswer : IOutPacket
    {
        public string MessageType;
        public string SenderCharacterName;
        public string Message;
        
        public Packet CreatePacket()
        {
            var ack = new Packet(Packets.ChatMsgAck);
            ack.Writer.Write(GetBytes());
            return ack;
        }

        public byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.WriteUnicodeStatic(MessageType, 10);
                    bs.WriteUnicodeStatic(SenderCharacterName, 18);
                    bs.WriteUnicode(Message);
                }
                return ms.GetBuffer();
            }
        }
    }
}