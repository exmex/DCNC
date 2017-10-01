using Shared.Network;

namespace AreaServer.Network.Handlers
{
    public static class UdpCastTraffic
    {
        [Packet(Packets.CmdUdpCastTraffic)]
        public static void Handle(Packet packet)
        {
        }
    }
}