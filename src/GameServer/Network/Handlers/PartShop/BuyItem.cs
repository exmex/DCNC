using Shared;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Util;

namespace GameServer.Network.Handlers
{
    public class BuyItem
    {
        [Packet(Packets.CmdBuyItem)]
        public static void Handle(Packet packet)
        {
            var buyItemPacket = new BuyItemPacket(packet);

            // Check if the item really exists
            if (buyItemPacket.TableIndex > ServerMain.Items.Count)
            {
                packet.Sender.SendDebugError($"Item {buyItemPacket.TableIndex} out of range!");
#if !DEBUG
                packet.Sender.KillConnection("Invalid shop item");
#endif
                return;
            }

            
            var itemData = ServerMain.Items[buyItemPacket.TableIndex];
#if DEBUG
            Log.Debug($"{itemData.Id} - {itemData.Name} - {buyItemPacket.TableIndex}");
#endif
            // Get price for single item
            int price;
            if (!int.TryParse(itemData.BuyValue, out price) || itemData.BuyValue == "n/a")
            {
                packet.Sender.SendDebugError($"No price ({itemData.BuyValue}) for item {itemData.Name}");
#if !DEBUG
                packet.Sender.KillConnection("Price missing!");
#endif
                return;
            }

            price = price * (int)buyItemPacket.Quantity;

            var character = packet.Sender.User.ActiveCharacter;

            // Check money
            if (character.MitoMoney < price)
            {
                packet.Sender.SendDebugError("Not enough money");
                return;
            }

            // Give the item to user
            var inventoryItem = character.GiveItem(GameServer.Instance.Database.Connection,
                buyItemPacket.TableIndex, buyItemPacket.Quantity);
            if (inventoryItem == null)
            {
                packet.Sender.SendDebugError("Giving item failed");
                return;
            }

            // Finally update money
            character.MitoMoney -= price;
            CharacterModel.Update(GameServer.Instance.Database.Connection, character);

            var ack = new BuyItemAnswer()
            {
                ItemId = buyItemPacket.TableIndex,
                Quantity = buyItemPacket.Quantity,
                Price = price,
            };
            packet.Sender.Send(ack.CreatePacket());
            
            character.FlushItemModBuffer(packet.Sender);
        }
    }
}