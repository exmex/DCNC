using Shared.Network;

namespace AuthServer.Network.Handlers
{
    public static class ServerMessage
    {
        /// <summary>
        /// Handles the CmdServerMessage packet
        /// </summary>
        /// <param name="packet">The packet</param>
        [Packet(Packets.CmdServerMessage)]
        public static void Handle(Packet packet){ /*Ignored*/ }
    }
}