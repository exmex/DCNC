using System.Linq;
using Shared;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Objects.GameDatas;
using Shared.Util;

namespace GameServer.Network.Handlers
{
    public class BuyVisualItemThread
    {
        //bool __cdecl PacketSend::Send_BuyVisualItem(BS_PacketDispatch *lpDispatch, XiCsItem *pItem, int Period, int Mito, int Hancoin, int BonusMito, int Mileage) <-- Type 1
        //bool __cdecl PacketSend::Send_BuyVisualItem(BS_PacketDispatch *lpDispatch, XiCsVSItem *pItem, int Period, int Mito, int Hancoin, int BonusMito, int Mileage) <-- Type 0
        [Packet(Packets.CmdBuyVisualItemThread)]
        public static void Handle(Packet packet)
        {
            // TODO: Jeez. Just refactor this whole fucking file. It's disgusting.
            var buyVisualItemPacket = new BuyVisualItemThreadPacket(packet);

            var item = ServerMain.VisualItems.FirstOrDefault(itm =>
            {
                int uniqueId;
                if (int.TryParse(itm.UniqueId, out uniqueId))
                    return uniqueId == buyVisualItemPacket.TableIndex;
                return false;
            });

            if (item == null)
            {
                packet.Sender.SendError("Failed to purchase item!");
                return;
            }

            int price;
            // We should probably fucking use another function for this crap. As it is so unorganized here.
            switch (buyVisualItemPacket.PeriodIdx)
            {
                default:
                    Log.Error("Invalid period id");
                    packet.Sender.SendError("Failed to purchase item!");
                    return;
                case 0u:
                    if (item.UseHancoin == "1")
                    {
                        Log.Error("Invalid period id");
                        packet.Sender.SendError("Failed to purchase item!");
                        return;
                    }
                    else
                    {
                        int.TryParse(item.MitoPrice, out price);
                    }
                    break;
                case 1u: // 7
                    if (item.UseHancoin == "1")
                        int.TryParse(item.Hancoin7dPrice, out price);
                    else
                        int.TryParse(item.Mito7dPrice, out price);
                    break;
                case 2u: // 30
                    if (item.UseHancoin == "1")
                        int.TryParse(item.Hancoin30dPrice, out price);
                    else
                        int.TryParse(item.Mito30dPrice, out price);
                    break;
                case 3u: // 90
                    if (item.UseHancoin == "1")
                    {
                        if (item.Hancoin90dPrice != null)
                        {
                            int.TryParse(item.Hancoin90dPrice, out price);
                        }
                        else
                        {
                            if (item.Hancoin365dPrice == null)
                            {
                                Log.Error("90d price and 365d price don't exist!");
                                packet.Sender.SendError("Failed to purchase item!");
                                return;
                            }
                            int.TryParse(item.Hancoin365dPrice, out price);
                        }
                    }
                    else
                    {
                        if (item.Mito90dPrice != null)
                        {
                            int.TryParse(item.Mito90dPrice, out price);
                        }
                        else
                        {
                            if (item.Mito365dPrice == null)
                            {
                                Log.Error("90d price and 365d price don't exist!");
                                packet.Sender.SendError("Failed to purchase item!");
                                return;
                            }
                            int.TryParse(item.Mito365dPrice, out price);
                        }
                    }
                        
                    break;
                case 4u: // 0
                    if (item.UseHancoin == "1")
                        int.TryParse(item.Hancoin0dPrice, out price);
                    else
                        int.TryParse(item.Mito0dPrice, out price);
                    break;
                case 5u: // Infinite (Doesn't even exist in leaked files.. Probably 0u changed to 5u)
                    if (item.UseHancoin == "1")
                        int.TryParse(item.Hancoin0dPrice, out price);
                    else
                        int.TryParse(item.MitoPrice, out price);
                    break;
            }

            int categoryIndex = 0;
            int.TryParse(item.CategoryIndex, out categoryIndex);
            // TODO: We should probably do a switch here?
            if (categoryIndex == 16) // Extend Inventory
            {
                // !! Don't use exact string and fix this broken english. !!
                /*
                BS_GameDB::GetAllHancoinInvenHistory(v12, v11, &Count);
                if ( (unsigned int)Count >= 8 )
                    PacketSend::Send_Error(lpDispatch->m_pSession, L"Can't take a extended inven.");
                */
            }
            else if (categoryIndex == 19) // Parts box give item i_n_00027
            {
            }
            else if (categoryIndex == 22) // Extend Garage
            {
                /*
                BS_GameDB::GetAllHancoinGarage(v14, v13, &v94);
                if ( BS_SingletonHeap<XiItemTable,5>::GetInstance()->m_nMaxLogicalFloor <= v94 )
                    PacketSend::Send_Error(lpDispatch->m_pSession, L"Can't take a extended garage.");
                */
            }

            var ack = new BuyVisualItemThreadAnswer();
            int.TryParse(item.Support, out ack.Type);
            ack.TableIndex = buyVisualItemPacket.TableIndex;
            ack.CarId = buyVisualItemPacket.CarId;
            ack.InventoryId = 0;
            ack.Period = (int) buyVisualItemPacket.PeriodIdx;
            if (item.UseHancoin == "1")
                ack.Hancoin = price;
            else
                ack.Mito = price;
            ack.BonusMito = 0;
            ack.Mileage = 0;
            packet.Sender.Send(ack.CreatePacket());

            var invenItem = ServerMain.Items.FirstOrDefault(basicItem => basicItem.Name == item.ItemName);

            var ack2 = new RoomNotifyChangeAnswer()
            {
                Serial = 0,
                Age = 0,
                CarAttr = new XiCarAttr(),
                PlayerInfo = new XiPlayerInfo(packet.Sender.User.VehicleSerial, packet.Sender.User.ActiveCharacter)
                {
                    VisualItem = new XiVisualItem()
                    {
                        Neon = 1,
                        Plate = 1,
                        Decal = 1,
                        DecalColor = 1,
                        AeroBumper = 1,
                        AeroIntercooler = 1,
                        AeroSet = 1,
                        MufflerFlame = 1,
                        Wheel = 1,
                        Spoiler = 1,
                        Reserve = new short[] {0, 0, 0, 0, 0, 0},
                        PlateString = "HELLO",
                    },
                    UseTime = 100.0f,
                }
            };
            packet.Sender.Send(ack2.CreatePacket());

            /*
            if ( pItem && VisualItemCategory::IsEnableEquip_0(pItem->m_pItem->categoryIndex) == 1 )
            {
                pEquipedItem = XiCsCharInfo::GetEquipVisualItem(pCharInfo, pItem->m_MyVSItem.CarID, pItem->m_pItem->categoryIndex);
                if ( pEquipedItem )
                    XiCsCharInfo::UnEquipVisualItem(pCharInfo, pEquipedItem->m_MyVSItem.InvenIdx, pEquipedItem->m_MyVSItem.CarID);
                XiCsCharInfo::EquipVisualItem(pCharInfo, pItem->m_MyVSItem.InvenIdx, pItem->m_MyVSItem.CarID);
            }
            PacketSend::Send_VSItemModList((BS_PacketDispatch *)&pGameDispatch->vfptr);
            PacketSend::Send_BuyVisualItem((BS_PacketDispatch *)&pGameDispatch->vfptr, pItem, v64, v66, Hancoin, Money, Mileage);
            PacketSend::Send_BonusUpdate((BS_PacketDispatch *)&pGameDispatch->vfptr);
            PacketSend::Send_VisualUpdate((BS_PacketDispatch *)&pGameDispatch->vfptr, lpBuyVisualItem->CarID);
            PacketSend::Send_StatUpdate((BS_PacketDispatch *)&pGameDispatch->vfptr);
            PacketSend::Send_PartyEnChantUpdateAll((BS_PacketDispatch *)&pGameDispatch->vfptr);
            */

            // TODO: Send visual update aka BS_PktRoomNotifyChange
/* BS_PktRoomNotifyChange

struct XiPlayerInfo
{
  wchar_t Cname[13];
  unsigned __int16 Serial;
  unsigned __int16 Age;
  __unaligned __declspec(align(1)) __int64 Cid;
  unsigned __int16 Level;
  unsigned int Exp;
  __unaligned __declspec(align(1)) __int64 TeamId;
  __unaligned __declspec(align(1)) __int64 TeamMarkId;
  wchar_t TeamName[14];
  unsigned __int16 TeamNLevel;
  XiVisualItem VisualItem;
  float UseTime;
};
*/
        }
    }
}