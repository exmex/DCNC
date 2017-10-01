using Shared.Network;

namespace GameServer.Network.Handlers.Join
{
    public class VisualItemList
    {
        [Packet(Packets.CmdVisualItemList)]
        public static void Handle(Packet packet)
        {
            var ack = new Packet(Packets.VisualItemListAck);
            ack.Writer.Write(262144); // ListUpdate (262144 = First packet from list queue, 262145 = sequential)
            //packet.Sender.User.ActiveCharacter.InventoryVisualItems.Count
            ack.Writer.Write(0); // ItemNum
            ack.Writer.Write(new byte[120]); // Null VisualItem (120 bytes per XiStrMyVSItem)
            packet.Sender.Send(ack);
        }
    }
}