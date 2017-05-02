using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.AreaServer
{
    public class TimeSyncPacket
    {
        public uint LocalTime;
        public TimeSyncPacket(Packet packet)
        {
            LocalTime = packet.Reader.ReadUInt32();
        }
    }
}
