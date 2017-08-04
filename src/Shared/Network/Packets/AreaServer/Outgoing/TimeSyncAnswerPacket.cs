namespace Shared.Network.AreaServer
{
    public class TimeSyncAnswerPacket
    {
        public uint GlobalTime;
        public uint SystemTick = 0;

        public void Send(Client client)
        {
            var ack = new Packet(Packets.UdpTimeSyncAck);
            ack.Writer.Write(GlobalTime); // Relay?
            ack.Writer.Write(SystemTick); // System Tick.
            client.Send(ack);
        }
    }
}