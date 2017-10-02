namespace Shared.Network.GameServer
{
    public class PlayerInfoReqPacket
    {
        public ushort[] VehicleSerials;
        
        public PlayerInfoReqPacket(Packet packet)
        {
            var reqCnt = packet.Reader.ReadUInt32();
            if ( reqCnt > 40) // bounds check.
                reqCnt = 40;
            VehicleSerials = new ushort[reqCnt];
            
            for (var i = 0; i < reqCnt; i++)
            {
                VehicleSerials[i] = packet.Reader.ReadUInt16();
                packet.Reader.ReadUInt16(); // Just discard age. I don't even know what this shit is?
            }
        }
    }
}