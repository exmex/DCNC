using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.AreaServer
{
    public class AreaChatAnswerPacket
    {
        public readonly string Type;
		
		public readonly string Sender;
		
		public readonly string Message;
		
        public void Send(Client client)
        {
			var ack = new Packet(Packets.AreaChatAck);
			
			ack.Writer.WriteUnicodeStatic(Type, 10);
            ack.Writer.WriteUnicodeStatic(Sender, 18);
            ack.Writer.WriteUnicode(Message);
			
			if(Type == "area")
			{
				AreaServer.Instance.Server.Broadcast(ack);
			}else{
				// Error missing type!
			}
        }
    }
}
