using Shared.Network;

namespace GameServer.Network.Handlers.Join
{
    public class AreaList
    {
        [Packet(Packets.CmdAreaList)]
        public static void Handle(Packet packet)
        {
            // client calls 2 functions (not using any packet data), returns  137 * (*(_DWORD *)(pktBuf + 2) - 1) + 143;
            var ack = new Packet(Packets.AreaListAck);
            ack.Writer.Write(1);
            ack.Writer.Write(new byte[137]);
            packet.Sender.Send(ack);
        }
    }
}