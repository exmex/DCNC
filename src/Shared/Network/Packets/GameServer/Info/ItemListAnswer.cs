using System;
using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// TODO: Packet not implemented
    /// </summary>
    public class ItemListAnswer : OutPacket
    {
        /*
        struct XiStrMyVSItem
        {
          unsigned int CarID;
          int ItemState;
          unsigned int TableIdx;
          unsigned int InvenIdx;
          XiStrPlateName PlateName;
          int Period;
          int UpdateTime;
          int CreateTime;
        };
        */
        public XiStrMyItemMod[] Items;
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.ItemListAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(0x40000); // 262144
                    foreach (var item in Items)
                    {
                        bs.Write(item.MyItem.CarID);
                        bs.Write(item.MyItem.Itm.State);
                        bs.Write(item.MyItem.Itm.Slot);
                        bs.Write(item.MyItem.Itm.StateVar);
                        bs.Write(item.MyItem.iunit.StackNum);
                        bs.Write(item.MyItem.iunit.Random);
                        bs.Write(item.MyItem.iunit.AssistA);
                        bs.Write(item.MyItem.iunit.AssistB);
                        bs.Write(item.MyItem.iunit.Box);
                        bs.Write(item.MyItem.iunit.Belonging);
                        bs.Write(item.MyItem.iunit.Upgrade);
                        bs.Write(item.MyItem.iunit.UpgradePoint);
                        bs.Write(item.MyItem.iunit.ExpireTick);
                        bs.Write(item.MyItem.TableIdx);
                        bs.Write(item.MyItem.InvenIdx);
                        bs.Write((uint)0);
                        bs.Write((uint)0);
                    }
                }
                #if DEBUG
                return ms.ToArray();
                #else
                return new byte[0];
                #endif
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