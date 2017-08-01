using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.AreaServer
{
    public class MoveVehicleAnswerPacket
    {
		// TODO: Split open
        public byte[] Movement;
		
		public ushort CarSerial;

        public void Send(Client client)
        {
            var ack = new Packet(Packets.MoveVehicleAck);
			ack.Writer.Write(CarSerial);
			ack.Writer.Write(Movement);
            AreaServer.Instance.Server.Broadcast(ack);
        }
    }
}
