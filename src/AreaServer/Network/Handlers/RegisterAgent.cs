using Shared.Network;

namespace AreaServer.Network.Handlers
{
    public static class RegisterAgent
    {
        [Packet(Packets.CmdRegisterAgent)]
        public static void Handle(Packet packet)
        {
        }
    }
}