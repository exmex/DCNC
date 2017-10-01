using Shared;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class SellItem
    {
        [Packet(Packets.CmdSellItem)]
        public static void Handle(Packet packet)
        {
            var sellItemPacket = new SellItemPacket(packet);
            
            // Check if the item really exists
            if (ServerMain.Items.Count < sellItemPacket.TableIndex)
            {
                packet.Sender.SendDebugError("Item out of range!");
#if !DEBUG
                packet.Sender.KillConnection("Invalid shop item");
#endif
                return;
            }
            
            // Get price for single item
            var itemData = ServerMain.Items[(int)sellItemPacket.TableIndex];
            uint price;
            if (!uint.TryParse(itemData.SellValue, out price) || itemData.BuyValue == "n/a")
            {
                packet.Sender.SendDebugError($"No sell price ({itemData.BuyValue}) for item {sellItemPacket.TableIndex}");
#if !DEBUG
                packet.Sender.KillConnection("Price missing!");
#endif
                return;
            }
            
            price = price * sellItemPacket.Quantity;

            var character = packet.Sender.User.ActiveCharacter;
            
            // Give the item to user
            if (!character.RemoveItem(GameServer.Instance.Database.Connection,
                (int)sellItemPacket.Slot, sellItemPacket.Quantity))
            {
                packet.Sender.SendDebugError("Removing item failure");
                return;
            }

            // Finally update money
            character.MitoMoney += price;
            CharacterModel.Update(GameServer.Instance.Database.Connection, character);
            
            packet.Sender.Send(new SellItemAnswer()
            {
                TableIndex = sellItemPacket.TableIndex,
                Quantity = sellItemPacket.Quantity,
                Money = price,
                Slot = sellItemPacket.Slot 
            }.CreatePacket());
            
            character.FlushItemModBuffer(packet.Sender);
        }
    }
}