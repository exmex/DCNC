using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public static class Messages
    {
        [Packet(Packets.CmdSendMail)]
        public static void SendMail(Packet packet)
        {
            var sendMailPacket = new SendMailPacket(packet);
            #if !DEBUG
            throw new NotImplementedException();
            #endif
        }
    }
}