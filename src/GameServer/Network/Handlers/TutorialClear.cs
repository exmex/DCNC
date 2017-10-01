using Shared.Network;

namespace GameServer.Network.Handlers
{
    public class TutorialClear
    {
        [Packet(Packets.CmdTutorialClear)]
        public static void Handle(Packet packet)
        {
            var type = packet.Reader.ReadUInt32();
            var ack = new Packet(Packets.TutorialClearAck);
            ack.Writer.Write(type);

            packet.Sender.Send(ack);
        }
    }
}