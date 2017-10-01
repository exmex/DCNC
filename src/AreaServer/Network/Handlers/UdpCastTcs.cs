using Shared.Network;

namespace AreaServer.Network.Handlers
{
    public static class UdpCastTcs
    {
        [Packet(Packets.CmdUdpCastTcs)]
        public static void Handle(Packet packet)
        {
            // Traffic?
        }
    }
}