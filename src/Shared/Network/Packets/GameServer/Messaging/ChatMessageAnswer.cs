using System.IO;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_539050
    /// </summary>
    public class ChatMessageAnswer : OutPacket
    {
        /// <summary>
        /// Message Type
        /// Referenced as m_Name
        /// Types:
        /// - channel = Channel Message
        /// - debug - Debug Messages
        /// </summary>
        public string MessageType;
        
        /// <summary>
        /// The character name of the sender
        /// Referenced as m_Player
        /// </summary>
        public string SenderCharacterName;
        
        /// <summary>
        /// The actual message
        /// ???
        /// </summary>
        public string Message = "MESSAGE";
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.ChatMsgAck);
        }
        
        public override int ExpectedSize() => 2*(Message.Length+30);

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
                return ms.ToArray();
            }
        }
    }
}