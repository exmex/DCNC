using System.IO;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class ChatMessageAnswer : OutPacket
    {
        public string MessageType;
        public string SenderCharacterName;
        public string Message;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.ChatMsgAck);
        }

        public override byte[] GetBytes()
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