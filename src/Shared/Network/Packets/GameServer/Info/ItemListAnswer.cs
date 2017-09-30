using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_52EAC0
    /// </summary>
    public class ItemListAnswer : OutPacket
    {
        public InventoryItem[] InventoryItems = new InventoryItem[0];
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.ItemListAck);
        }
        
        public override int ExpectedSize() => (96 * InventoryItems.Length-1) + 106;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(262144); // First packet, 262145 would be 2nd, 3rd etc.
                    bs.Write(InventoryItems.Length);
                    foreach (var item in InventoryItems)
                    {
                        bs.Write(item);
                    }
                }
                return ms.ToArray();
            }
            
            /*
            var ack = new Packet(Packets.ItemListAck);
            ack.Writer.Write(
                (uint) 0x40000); // WHAT THE? unsigned int ListUpdate; <-- Multiple pages -> Multiple packets
            //ack.Writer.Write((uint)0); // Count
            ack.Writer.Write((uint) 1); // Count
            // 52 bytes of data for each item

            ack.Writer.Write((uint) 4); // CarID
            ack.Writer.Write((ushort) 0); // State // (0 = Inventory, 1 = Equip, 4 = Auction)
            ack.Writer.Write((ushort) 1); // Slot
            ack.Writer.Write((uint) 1); // StateVar
            ack.Writer.Write(1); // StackNum
            ack.Writer.Write(0); // Random

            ack.Writer.Write((uint) 0); // AssistA
            ack.Writer.Write((uint) 0); // AssistB
            ack.Writer.Write((uint) 0); // Box // 255 = drop rate 5.7???
            ack.Writer.Write((uint) 0); // Belonging (0 = , 1 = equip, 2 = get)
            ack.Writer.Write(0); // Upgrade
            ack.Writer.Write(0); // UpgradePoint
            ack.Writer.Write((uint) 0); // ExpireTick
            ack.Writer.Write((uint) 1); // TableIdx
            ack.Writer.Write((uint) 0); // InvenIdx
            ack.Writer.Write((uint) 0); // Is upgrade pack??
            ack.Writer.Write((uint) 0); // Tradeable?
            //ack.Writer.Write(new byte[46]); // 94

            /* Pulled from Rice:
            byte[] testItem = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69, 0x91, 0x00, 0x00, 0x00, 0x00, 0x80, 0xBF, 
    0x00, 0x00, 0x00, 0x00, 0xD9, 0x04, 0x00, 0x00, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
            var ack = new RicePacket(401);
            ack.Writer.Write(262144); // ListUpdate (262144 = First packet from list queue, 262145 = sequential)
            ack.Writer.Write(1); // ItemNum
            ack.Writer.Write(testItem); // Null Item (96 bytes per XiStrMyItem)
            packet.Sender.Send(ack);
            */
        }
    }
}