using Shared.Network;

namespace GameServer.Network.Handlers.Social
{
    public class SendMail
    {
        //[Packet(Packets.CmdSendMail)]
        public static void Handle(Packet packet)
        {
#if !DEBUG
            Log.Unimplemented("Not implemented");
#endif
        }
    }
}