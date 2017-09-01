namespace Shared.Network.GameServer.Tutorial
{
    public static class Tutorial
    {
        [Packet(Packets.CmdTutorialClear)]
        public static void TutorialClear(Packet packet)
        {
            var type = packet.Reader.ReadUInt32();
            var ack = new Packet(Packets.TutorialClearAck);
            ack.Writer.Write(type);
            
            packet.Sender.Send(ack);
        }
    }
}