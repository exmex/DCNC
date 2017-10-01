using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers.Join
{
    public class ItemList
    {
        [Packet(Packets.CmdItemList)]
        public static void Handle(Packet packet)
        {
            ItemModel.RetrieveAll(GameServer.Instance.Database.Connection,
                ref packet.Sender.User.ActiveCharacter);
            
            var items = ItemModel.RetrieveAll(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId);
            var ack = new ItemListAnswer
            {
                InventoryItems = items.ToArray()
            };

            packet.Sender.Send(ack.CreatePacket());
        }
    }
}