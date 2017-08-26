using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class ItemModListAnswer : OutPacket
    {
        public XiStrMyItemMod[] Items;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.BuyItemAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Items.Length);
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
                        bs.Write(item.State);
                    }
                    /*ack.Writer.Write(1); // ItemNum

                    ack.Writer.Write((uint) 4); // CarID
                    ack.Writer.Write((ushort) 1); // State
                    ack.Writer.Write((ushort) 1); // Slot
                    ack.Writer.Write((uint) 1); // StateVar
                    ack.Writer.Write(quantity); // StackNum
                    ack.Writer.Write(0); // Random

                    ack.Writer.Write((uint) 0); // AssistA
                    ack.Writer.Write((uint) 0); // AssistB
                    ack.Writer.Write((uint) 0); // Box
                    ack.Writer.Write((uint) 0); // Belonging
                    ack.Writer.Write(0); // Upgrade
                    ack.Writer.Write(0); // UpgradePoint
                    ack.Writer.Write((uint) 0); // ExpireTick
                    ack.Writer.Write((uint) itemID); // TableIdx
                    ack.Writer.Write((uint) 0); // InvenIdx

                    ack.Writer.Write(0); // State
                    */
                }
                return ms.GetBuffer();
            }
        }
    }
}