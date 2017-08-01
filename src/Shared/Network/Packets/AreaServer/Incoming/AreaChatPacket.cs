using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.AreaServer
{
    public class AreaChatPacket
    {
        public readonly string Type;
		
		public readonly string Sender;
		
		public readonly string Message;

        public AreaChatPacket(Packet packet)
        {
			Type = packet.Reader.ReadUnicodeStatic(10);
            Sender = packet.Reader.ReadUnicodeStatic(18);
            Message = packet.Reader.ReadUnicodePrefixed();
        }
    }
}
