using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.AreaServer
{
    public class MoveVehiclePacket
    {
        public readonly ushort SessionId;
		
		// TODO: Split open
		public readonly byte[] Movement;

        public MoveVehiclePacket(Packet packet)
        {
            SessionId = packet.Reader.ReadInt16();
			Movement = packet.Reader.ReadBytes(112);
        }
    }
}
