using System.Linq;
using Shared;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Util;

namespace GameServer.Network.Handlers
{
    public class DriftShop
    {
/*
[Info] - Received unhandled packet UpgradeCar (CmdUpgradeCarThread id 91, 0x5B) on 11021.
[Debug] - HexDump  UpgradeCar (CmdUpgradeCarThread id 91, 0x5B):
000000: 15 00 00 00 10 27 00 00 00 00 00 00 00 00 00 00  · · · · · ' · · · · · · · · · ·
000016: 00  ·
*/
        [Packet(Packets.CmdUpgradeCarThread)]
        public static void UpgradeCarThread(Packet packet)
        {
            // TODO: Implement Uprgade Car.
        }

        //bool __cdecl PacketSend::Send_BuyVisualItem(BS_PacketDispatch *lpDispatch, XiCsItem *pItem, int Period, int Mito, int Hancoin, int BonusMito, int Mileage) <-- Type 1
        //bool __cdecl PacketSend::Send_BuyVisualItem(BS_PacketDispatch *lpDispatch, XiCsVSItem *pItem, int Period, int Mito, int Hancoin, int BonusMito, int Mileage) <-- Type 0
        [Packet(Packets.CmdBuyVisualItemThread)]
        public static void BuyVisualItemThread(Packet packet)
        {
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
            switch (buyVisualItemPacket.PeriodIdx)
            {
                default:
                    Log.Error("Invalid period id");
                    packet.Sender.SendError("Failed to purchase item!");
                    return;
                case 1: // 7
                    if (item.UseHancoin == "1")
                        int.TryParse(item.Hancoin7dPrice, out price);
                    else
                        int.TryParse(item.Mito7dPrice, out price);
                    break;
                case 2: // 30
                    if (item.UseHancoin == "1")
                        int.TryParse(item.Hancoin30dPrice, out price);
                    else
                        int.TryParse(item.Mito30dPrice, out price);
                    break;
                case 3: // 90
                    if (item.UseHancoin == "1")
                        int.TryParse(item.Hancoin90dPrice, out price);
                    else
                        int.TryParse(item.Mito90dPrice, out price);
                    break;
                case 4: // 0
                    if (item.UseHancoin == "1")
                        int.TryParse(item.Hancoin0dPrice, out price);
                    else
                        int.TryParse(item.Mito0dPrice, out price);
                    break;
                case 5: // Infinite
                    if (item.UseHancoin == "1")
                      int.TryParse(item.Hancoin0dPrice, out price);
                    else
                        int.TryParse(item.MitoPrice, out price);
                    break;
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
          
            //if ( pItemInfo->categoryIndex == 19 ) <-- Parts box give item i_n_00027
          
            var ack2 = new RoomNotifyChangeAnswer()
            {
                Serial = 0,
                Age = 0,
                CarAttr = new XiCarAttr(),
                PlayerInfo = new XiPlayerInfo()
                {
                    CharacterName = packet.Sender.User.ActiveCharacter.Name,
                    Serial = 0,
                    Age = 4,
                    Level = packet.Sender.User.ActiveCharacter.Level,
                    Exp = 0, // ??
                    TeamId = packet.Sender.User.ActiveCharacter.TeamId,
                    TeamMarkId = packet.Sender.User.ActiveCharacter.Team.MarkId,
                    TeamName = packet.Sender.User.ActiveCharacter.Team.Name,
                    TeamNLevel = 0,
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
/*
unsigned __int16 __cdecl PacketParser::Cmd_BuyVisualItem(BS_PacketDispatch *lpDispatch, char *lpBuffer)
{
  BS_Session *lpSession; // ST200_4@2
  wchar_t *v3; // eax@2
  BS_Session *v5; // ST1FC_4@4
  wchar_t *v6; // eax@4
  XiVisualItemTable *v7; // eax@5
  XiVisualItemTable *v8; // eax@7
  BS_Session *v9; // ST1C8_4@11
  wchar_t *v10; // eax@11
  __int64 v11; // ST10_8@15
  BS_GameDB *v12; // eax@15
  __int64 v13; // ST10_8@18
  BS_GameDB *v14; // eax@18
  BS_Session *v15; // ST180_4@24
  wchar_t *v16; // eax@24
  XiDBOperation *v17; // eax@49
  XiDBUpdateMgr *v18; // eax@51
  BS_Session *v19; // ST148_4@65
  wchar_t *v20; // eax@65
  __int64 v21; // ST14_8@87
  int v22; // ST10_4@87
  const char *v23; // ST08_4@87
  const char *v24; // eax@87
  const char *v25; // eax@88
  const char *v26; // eax@89
  BS_Session *v27; // ST11C_4@89
  wchar_t *v28; // eax@89
  BS_Session *v29; // STFC_4@112
  wchar_t *v30; // eax@112
  const char *v31; // eax@113
  BS_Session *v32; // STF0_4@114
  wchar_t *v33; // eax@114
  XiItemTable *v34; // eax@119
  BS_Session *v35; // STDC_4@120
  wchar_t *v36; // eax@120
  BS_Session *v37; // STD8_4@122
  wchar_t *v38; // eax@122
  const wchar_t *v39; // ST14_4@150
  __int64 v40; // ST0C_8@150
  BS_GameDB *v41; // eax@150
  BS_Session *v42; // STAC_4@163
  wchar_t *v43; // eax@163
  XiCsVSItem *v44; // eax@165
  BS_Session *v45; // STA4_4@176
  wchar_t *v46; // eax@176
  const wchar_t *v47; // ST14_4@204
  __int64 v48; // ST0C_8@204
  BS_GameDB *v49; // eax@204
  unsigned int v50; // [sp+4h] [bp-438h]@202
  signed int v51; // [sp+20h] [bp-41Ch]@148
  XiDBOperation *v52; // [sp+3Ch] [bp-400h]@49
  int v53; // [sp+9Ch] [bp-3A0h]@152
  unsigned int v54; // [sp+ECh] [bp-350h]@100
  unsigned int v55; // [sp+104h] [bp-338h]@77
  int v56; // [sp+130h] [bp-30Ch]@54
  int v57; // [sp+15Ch] [bp-2E0h]@36
  bool v58; // [sp+163h] [bp-2D9h]@27
  unsigned __int16 v59; // [sp+1E8h] [bp-254h]@114
  unsigned __int16 v60; // [sp+1EAh] [bp-252h]@89
  XiUpdateCharMileage *v61; // [sp+1ECh] [bp-250h]@48
  XiCsVSItem *pEquipedItem; // [sp+1F4h] [bp-248h]@206
  XiStrVisualItemBuyUnit BuyUnit; // [sp+1F8h] [bp-244h]@201
  int v64; // [sp+2D0h] [bp-16Ch]@191
  XiCsVSItem *pItem; // [sp+2D4h] [bp-168h]@175
  int v66; // [sp+2D8h] [bp-164h]@162
  __int64 Money; // [sp+2DCh] [bp-160h]@147
  int v68; // [sp+2E4h] [bp-158h]@147
  unsigned int v69; // [sp+2E8h] [bp-154h]@147
  int v70; // [sp+2ECh] [bp-150h]@147
  int v71; // [sp+2F0h] [bp-14Ch]@147
  int v72; // [sp+2F4h] [bp-148h]@147
  signed int v73; // [sp+2F8h] [bp-144h]@150
  unsigned int v74; // [sp+2FCh] [bp-140h]@147
  int v75; // [sp+300h] [bp-13Ch]@150
  int v76; // [sp+304h] [bp-138h]@150
  int v77; // [sp+308h] [bp-134h]@150
  int v78; // [sp+30Ch] [bp-130h]@150
  __int16 v79; // [sp+310h] [bp-12Ch]@147
  __int16 v80; // [sp+33Ah] [bp-102h]@147
  int v81; // [sp+33Eh] [bp-FEh]@150
  int v82; // [sp+342h] [bp-FAh]@150
  int v83; // [sp+346h] [bp-F6h]@150
  int v84; // [sp+34Ah] [bp-F2h]@150
  __int16 v85; // [sp+34Eh] [bp-EEh]@147
  BS_StrUtils::BS_WidetoMB szIID; // [sp+3C0h] [bp-7Ch]@113
  XiCsItem *pBoxItem; // [sp+3C8h] [bp-74h]@121
  unsigned int Period; // [sp+3CCh] [bp-70h]@137
  __int64 Mito; // [sp+3D0h] [bp-6Ch]@110
  XiStrItem *pStrItem; // [sp+3D8h] [bp-64h]@119
  unsigned int BonusMito; // [sp+3DCh] [bp-60h]@124
  BS_StrUtils::BS_WidetoMB ItemCode; // [sp+3E0h] [bp-5Ch]@68
  BS_StrUtils::BS_WidetoMB ItemID; // [sp+3E8h] [bp-54h]@76
  int v94; // [sp+3F0h] [bp-4Ch]@18
  int Count; // [sp+3F4h] [bp-48h]@15
  XiStrCarInfo *pStrCarInfo; // [sp+3F8h] [bp-44h]@10
  const unsigned __int16 CmdLen; // [sp+3FCh] [bp-40h]@1
  BS_Game *pGame; // [sp+400h] [bp-3Ch]@1
  XIVISUALITEM_INFO *pItemInfo; // [sp+404h] [bp-38h]@7
  BS_GameDispatch *pGameDispatch; // [sp+408h] [bp-34h]@1
  unsigned int TableIdx; // [sp+40Ch] [bp-30h]@5
  int Hancoin; // [sp+410h] [bp-2Ch]@34
  XiVShop::ITEM_INFO *pVShopItemInfo; // [sp+414h] [bp-28h]@5
  bool UseMileage; // [sp+41Bh] [bp-21h]@5
  XiCsCharInfo *pCharInfo; // [sp+41Ch] [bp-20h]@3
  int Mileage; // [sp+420h] [bp-1Ch]@34
  unsigned int PeriodIdx; // [sp+424h] [bp-18h]@20
  BS_PktBuyVisualItem *lpBuyVisualItem; // [sp+428h] [bp-14h]@1
  XiStrPlateName *PlateName; // [sp+42Ch] [bp-10h]@5
  int v110; // [sp+438h] [bp-4h]@48

  lpBuyVisualItem = (BS_PktBuyVisualItem *)lpBuffer;
  CmdLen = 43;
  pGameDispatch = (BS_GameDispatch *)lpDispatch;
  pGame = (BS_Game *)lpDispatch[2].m_pRecvBuffer;
  if ( !pGame )
  {
    lpSession = lpDispatch->m_pSession;
    v3 = GTW("20017");
    PacketSend::Send_Error(lpSession, v3);
    return CmdLen;
  }
  pCharInfo = pGame->m_pCharInfo;
  if ( !pCharInfo )
  {
    v5 = lpDispatch->m_pSession;
    v6 = GTW("20017");
    PacketSend::Send_Error(v5, v6);
    return CmdLen;
  }
  TableIdx = lpBuyVisualItem->TableIdx;
  PlateName = &lpBuyVisualItem->PlateName;
  UseMileage = lpBuyVisualItem->UseMileage;
  v7 = BS_SingletonHeap<XiVisualItemTable,5>::GetInstance();
  pVShopItemInfo = XiVisualItemTable::GetVShopItemInfo(v7, TableIdx);
  if ( !pVShopItemInfo )
    return CmdLen;
  v8 = BS_SingletonHeap<XiVisualItemTable,5>::GetInstance();
  pItemInfo = XiVisualItemTable::GetVisualItemInfo(v8, TableIdx);
  if ( !pItemInfo )
    return CmdLen;
  if ( VisualItemCategory::IsEnableEquip_0(pItemInfo->categoryIndex) == 1 )
  {
    pStrCarInfo = XiCsCharInfo::FindCar(pCharInfo, lpBuyVisualItem->CarID);
    if ( !pStrCarInfo )
    {
      v9 = lpDispatch->m_pSession;
      v10 = GTW("20050");
      PacketSend::Send_Error(v9, v10);
      return CmdLen;
    }
    if ( pStrCarInfo->AuctionOn == 1 )
    {
      PacketSend::Send_Error(lpDispatch->m_pSession, &off_6BBD78);
      return CmdLen;
    }
  }
  if ( pItemInfo->categoryIndex == 16 )
  {
    Count = 0;
    v11 = pCharInfo->m_CharInfo.Cid;
    v12 = BS_SingletonHeap<BS_GameDB,1>::GetInstance();
    BS_GameDB::GetAllHancoinInvenHistory(v12, v11, &Count);
    if ( (unsigned int)Count >= 8 )
    {
      PacketSend::Send_Error(lpDispatch->m_pSession, L"Can't take a extended inven.");
      return CmdLen;
    }
  }
  if ( pItemInfo->categoryIndex == 22 )
  {
    v94 = 0;
    v13 = pCharInfo->m_CharInfo.Cid;
    v14 = BS_SingletonHeap<BS_GameDB,1>::GetInstance();
    BS_GameDB::GetAllHancoinGarage(v14, v13, &v94);
    if ( BS_SingletonHeap<XiItemTable,5>::GetInstance()->m_nMaxLogicalFloor <= v94 )
    {
      PacketSend::Send_Error(lpDispatch->m_pSession, L"Can't take a extended garage.");
      return CmdLen;
    }
  }
  PeriodIdx = lpBuyVisualItem->PeriodIdx;
  if ( (*(_BYTE *)&BS_Global::ContentsFlag.Etc.0 & 0x3Fu) < pVShopItemInfo->nSellable )
  {
    PacketSend::Send_Error(lpDispatch->m_pSession, &off_6BBE3C);
    return CmdLen;
  }
  if ( UseMileage && !pVShopItemInfo->bUseMileage )
  {
    v15 = lpDispatch->m_pSession;
    v16 = GTW("20050");
    PacketSend::Send_Error(v15, v16);
    return CmdLen;
  }
  if ( pVShopItemInfo->bUseHancoin == 1 && pVShopItemInfo->dwPriceHancoin000Day )
    v58 = 1;
  else
    v58 = pVShopItemInfo->bUseMito == 1 && pVShopItemInfo->dwPriceMito;
  if ( v58 == 1 )
    PeriodIdx = 4;
  Hancoin = 0;
  Mileage = 0;
  if ( UseMileage )
  {
    switch ( lpBuyVisualItem->PeriodIdx )
    {
      case 0u:
        v57 = 0;
        break;
      case 1u:
        v57 = pVShopItemInfo->dwPriceMileage7Day;
        break;
      case 2u:
        v57 = pVShopItemInfo->dwPriceMileage30Day;
        break;
      case 3u:
        if ( pVShopItemInfo->dwPriceMileage90Day )
        {
          v57 = pVShopItemInfo->dwPriceMileage90Day;
        }
        else
        {
          if ( !pVShopItemInfo->dwPriceMileage365Day )
            goto LABEL_45;
          v57 = pVShopItemInfo->dwPriceMileage365Day;
        }
        break;
      case 4u:
        v57 = pVShopItemInfo->dwPriceMileage000Day;
        break;
      default:
LABEL_45:
        v57 = 0;
        break;
    }
    Mileage = v57;
    if ( pCharInfo->m_CharInfo.Mileage < v57 )
    {
      PacketSend::Send_Error(lpDispatch->m_pSession, &off_6BBE58);
      return CmdLen;
    }
    pCharInfo->m_CharInfo.Mileage -= Mileage;
    v61 = (XiUpdateCharMileage *)operator new(0x20u);
    v110 = 0;
    if ( v61 )
    {
      XiUpdateCharMileage::XiUpdateCharMileage(v61, pCharInfo->m_CharInfo.Mileage, pCharInfo->m_CharInfo.Cid);
      v52 = v17;
    }
    else
    {
      v52 = 0;
    }
    v110 = -1;
    v18 = BS_SingletonHeap<XiDBUpdateMgr,2>::GetInstance();
    XiDBUpdateMgr::QueueDBOperation(v18, v52, 0);
    goto LABEL_98;
  }
  if ( pVShopItemInfo->bUseHancoin != 1 )
    goto LABEL_98;
  switch ( PeriodIdx )
  {
    case 0u:
      v56 = 0;
      break;
    case 1u:
      v56 = pVShopItemInfo->dwPriceHancoin7Day;
      break;
    case 2u:
      v56 = pVShopItemInfo->dwPriceHancoin30Day;
      break;
    case 3u:
      if ( pVShopItemInfo->dwPriceHancoin90Day )
      {
        v56 = pVShopItemInfo->dwPriceHancoin90Day;
      }
      else
      {
        if ( !pVShopItemInfo->dwPriceHancoin365Day )
          goto LABEL_63;
        v56 = pVShopItemInfo->dwPriceHancoin365Day;
      }
      break;
    case 4u:
      v56 = pVShopItemInfo->dwPriceHancoin000Day;
      break;
    default:
LABEL_63:
      v56 = 0;
      break;
  }
  Hancoin = v56;
  if ( !v56 )
  {
    v19 = lpDispatch->m_pSession;
    v20 = GTW("20134");
    PacketSend::Send_Error(v19, v20);
    return CmdLen;
  }
  if ( BS_SingletonHeap<BS_Config,5>::GetInstance()->m_bLocalBillingOn == 1 )
  {
LABEL_98:
    if ( pItemInfo->categoryIndex == 19 )
    {
      switch ( PeriodIdx )
      {
        case 0u:
          v54 = pVShopItemInfo->dwPriceMito;
          break;
        case 1u:
          v54 = pVShopItemInfo->dwPriceMito7Day;
          break;
        case 2u:
          v54 = pVShopItemInfo->dwPriceMito30Day;
          break;
        case 3u:
          if ( pVShopItemInfo->dwPriceMito90Day )
          {
            v54 = pVShopItemInfo->dwPriceMito90Day;
          }
          else
          {
            if ( !pVShopItemInfo->dwPriceMito365Day )
              goto LABEL_109;
            v54 = pVShopItemInfo->dwPriceMito365Day;
          }
          break;
        case 4u:
          v54 = pVShopItemInfo->dwPriceMito000Day;
          break;
        default:
LABEL_109:
          v54 = 0;
          break;
      }
      Mito = v54;
      if ( v54 )
      {
        if ( pCharInfo->m_CharInfo.MitoMoney < Mito )
        {
          v29 = lpDispatch->m_pSession;
          v30 = GTW("20029");
          PacketSend::Send_Error(v29, v30);
          return CmdLen;
        }
        BS_StrUtils::BS_WidetoMB::BS_WidetoMB(&szIID, pItemInfo->id, 0);
        v110 = 3;
        v31 = std::vector<XiVShop::ITEM_CATEGORY,std::allocator<XiVShop::ITEM_CATEGORY>>::const_iterator::operator*(&szIID);
        if ( !XiCsCharInfo::CalcMoney(pCharInfo, -Mito, "BUYVSITEM", "O", v31, byte_6BBEA6) )
        {
          v32 = lpDispatch->m_pSession;
          v33 = GTW("20029");
          PacketSend::Send_Error(v32, v33);
          v59 = CmdLen;
          v110 = -1;
          if ( szIID.m_pBuffer )
            operator delete[](szIID.m_pBuffer);
          return v59;
        }
        v110 = -1;
        if ( szIID.m_pBuffer )
          operator delete[](szIID.m_pBuffer);
      }
      v34 = BS_SingletonHeap<XiItemTable,5>::GetInstance();
      pStrItem = XiItemTable::GetItemByID(v34, L"i_n_00027");
      if ( !pStrItem )
      {
        v35 = lpDispatch->m_pSession;
        v36 = GTW("20029");
        PacketSend::Send_Error(v35, v36);
        return CmdLen;
      }
      pBoxItem = XiCsCharInfo::TakeItem(pCharInfo, pStrItem, 0xAu);
      if ( !pBoxItem )
      {
        v37 = lpDispatch->m_pSession;
        v38 = GTW("20029");
        PacketSend::Send_Error(v37, v38);
        return CmdLen;
      }
      switch ( PeriodIdx )
      {
        case 0u:
          BonusMito = 0;
          break;
        case 1u:
          BonusMito = pVShopItemInfo->dwBonusMito7Day;
          break;
        case 2u:
          BonusMito = pVShopItemInfo->dwBonusMito30Day;
          break;
        case 3u:
          if ( pVShopItemInfo->dwBonusMito90Day )
          {
            BonusMito = pVShopItemInfo->dwBonusMito90Day;
          }
          else
          {
            if ( !pVShopItemInfo->dwBonusMito365Day )
              goto LABEL_133;
            BonusMito = pVShopItemInfo->dwBonusMito365Day;
          }
          break;
        case 4u:
          BonusMito = pVShopItemInfo->dwBonusMito000Day;
          break;
        default:
LABEL_133:
          BonusMito = 0;
          break;
      }
      if ( BonusMito )
        XiCsCharInfo::CalcMoney(pCharInfo, BonusMito, "HANCOINBONUS", "O", "NONE", byte_6BBEB6);
      switch ( PeriodIdx )
      {
        case 0u:
          Period = 0;
          break;
        case 1u:
          Period = pVShopItemInfo->dwPeriod7Day;
          break;
        case 2u:
          Period = pVShopItemInfo->dwPeriod30Day;
          break;
        case 3u:
          if ( pVShopItemInfo->dwPeriod90Day )
          {
            Period = pVShopItemInfo->dwPeriod90Day;
          }
          else
          {
            if ( !pVShopItemInfo->dwPeriod365Day )
              goto LABEL_146;
            Period = pVShopItemInfo->dwPeriod365Day;
          }
          break;
        case 4u:
          Period = pVShopItemInfo->dwPeriod000Day;
          break;
        default:
LABEL_146:
          Period = 0;
          break;
      }
      v79 = 0;
      v80 = 0;
      v85 = 0;
      HIDWORD(Money) = 0;
      v68 = 0;
      v69 = TableIdx;
      v70 = GetTime_t();
      v71 = GetTime_t();
      v74 = Period;
      v72 = 0;
      if ( UseMileage == 1 )
        v51 = 2;
      else
        v51 = Hancoin == 0;
      v73 = v51;
      v76 = Mito;
      v75 = Hancoin;
      v77 = Mileage;
      *(_DWORD *)&v80 = *(_DWORD *)&PlateName->m_Name[0];
      v81 = *(_DWORD *)&PlateName->m_Name[2];
      v82 = *(_DWORD *)&PlateName->m_Name[4];
      v83 = *(_DWORD *)&PlateName->m_Name[6];
      v84 = *(_DWORD *)&PlateName->m_Name[8];
      v78 = 0;
      XiCsCharInfo::AddVisualItemBuyHistory(pCharInfo, (XiStrVisualItemBuyUnit *)((char *)&Money + 4));
      v39 = pCharInfo->m_CharInfo.Name.m_Name;
      v40 = pCharInfo->m_CharInfo.Cid;
      v41 = BS_SingletonHeap<BS_GameDB,1>::GetInstance();
      BS_GameDB::InsertVisualItemBuyHistory(v41, v40, v39, (XiStrVisualItemBuyUnit *)((char *)&Money + 4));
      PacketSend::Send_ItemModList((BS_PacketDispatch *)&pGameDispatch->vfptr);
      PacketSend::Send_BuyVisualItem(
        (BS_PacketDispatch *)&pGameDispatch->vfptr,
        pBoxItem,
        Period,
        Mito,
        Hancoin,
        BonusMito,
        Mileage);
    }
    else
    {
      switch ( PeriodIdx )
      {
        case 0u:
          v53 = pVShopItemInfo->dwPriceMito;
          break;
        case 1u:
          v53 = pVShopItemInfo->dwPriceMito7Day;
          break;
        case 2u:
          v53 = pVShopItemInfo->dwPriceMito30Day;
          break;
        case 3u:
          if ( pVShopItemInfo->dwPriceMito90Day )
          {
            v53 = pVShopItemInfo->dwPriceMito90Day;
          }
          else
          {
            if ( !pVShopItemInfo->dwPriceMito365Day )
              goto LABEL_161;
            v53 = pVShopItemInfo->dwPriceMito365Day;
          }
          break;
        case 4u:
          v53 = pVShopItemInfo->dwPriceMito000Day;
          break;
        default:
LABEL_161:
          v53 = 0;
          break;
      }
      v66 = v53;
      if ( pCharInfo->m_CharInfo.MitoMoney < v53 )
      {
        v42 = lpDispatch->m_pSession;
        v43 = GTW("20029");
        PacketSend::Send_Error(v42, v43);
        return CmdLen;
      }
      switch ( PeriodIdx )
      {
        case 0u:
          v44 = XiCsCharInfo::BuyVisualItem(
                  pCharInfo,
                  pItemInfo,
                  pVShopItemInfo,
                  lpBuyVisualItem->CarID,
                  PlateName->m_Name,
                  0,
                  &v66);
          break;
        case 1u:
          v44 = XiCsCharInfo::BuyVisualItem(
                  pCharInfo,
                  pItemInfo,
                  pVShopItemInfo,
                  lpBuyVisualItem->CarID,
                  PlateName->m_Name,
                  pVShopItemInfo->dwPeriod7Day,
                  &v66);
          break;
        case 2u:
          v44 = XiCsCharInfo::BuyVisualItem(
                  pCharInfo,
                  pItemInfo,
                  pVShopItemInfo,
                  lpBuyVisualItem->CarID,
                  PlateName->m_Name,
                  pVShopItemInfo->dwPeriod30Day,
                  &v66);
          break;
        case 3u:
          if ( pVShopItemInfo->dwPeriod90Day )
          {
            v44 = XiCsCharInfo::BuyVisualItem(
                    pCharInfo,
                    pItemInfo,
                    pVShopItemInfo,
                    lpBuyVisualItem->CarID,
                    PlateName->m_Name,
                    pVShopItemInfo->dwPeriod90Day,
                    &v66);
          }
          else
          {
            if ( !pVShopItemInfo->dwPeriod365Day )
              goto LABEL_174;
            v44 = XiCsCharInfo::BuyVisualItem(
                    pCharInfo,
                    pItemInfo,
                    pVShopItemInfo,
                    lpBuyVisualItem->CarID,
                    PlateName->m_Name,
                    pVShopItemInfo->dwPeriod365Day,
                    &v66);
          }
          break;
        case 4u:
          v44 = XiCsCharInfo::BuyVisualItem(
                  pCharInfo,
                  pItemInfo,
                  pVShopItemInfo,
                  lpBuyVisualItem->CarID,
                  PlateName->m_Name,
                  pVShopItemInfo->dwPeriod000Day,
                  &v66);
          break;
        default:
LABEL_174:
          v44 = XiCsCharInfo::BuyVisualItem(
                  pCharInfo,
                  pItemInfo,
                  pVShopItemInfo,
                  lpBuyVisualItem->CarID,
                  PlateName->m_Name,
                  0,
                  &v66);
          break;
      }
      pItem = v44;
      if ( !v44 )
      {
        v45 = lpDispatch->m_pSession;
        v46 = GTW("20029");
        PacketSend::Send_Error(v45, v46);
        return CmdLen;
      }
      switch ( PeriodIdx )
      {
        case 0u:
          LODWORD(Money) = 0;
          break;
        case 1u:
          LODWORD(Money) = pVShopItemInfo->dwBonusMito7Day;
          break;
        case 2u:
          LODWORD(Money) = pVShopItemInfo->dwBonusMito30Day;
          break;
        case 3u:
          if ( pVShopItemInfo->dwBonusMito90Day )
          {
            LODWORD(Money) = pVShopItemInfo->dwBonusMito90Day;
          }
          else
          {
            if ( !pVShopItemInfo->dwBonusMito365Day )
              goto LABEL_187;
            LODWORD(Money) = pVShopItemInfo->dwBonusMito365Day;
          }
          break;
        case 4u:
          LODWORD(Money) = pVShopItemInfo->dwBonusMito000Day;
          break;
        default:
LABEL_187:
          LODWORD(Money) = 0;
          break;
      }
      if ( (_DWORD)Money )
        XiCsCharInfo::CalcMoney(pCharInfo, (unsigned int)Money, "HANCOINBONUS", "O", "NONE", byte_6BBED1);
      switch ( PeriodIdx )
      {
        case 0u:
          v64 = 0;
          break;
        case 1u:
          v64 = pVShopItemInfo->dwPeriod7Day;
          break;
        case 2u:
          v64 = pVShopItemInfo->dwPeriod30Day;
          break;
        case 3u:
          if ( pVShopItemInfo->dwPeriod90Day )
          {
            v64 = pVShopItemInfo->dwPeriod90Day;
          }
          else
          {
            if ( !pVShopItemInfo->dwPeriod365Day )
              goto LABEL_200;
            v64 = pVShopItemInfo->dwPeriod365Day;
          }
          break;
        case 4u:
          v64 = pVShopItemInfo->dwPeriod000Day;
          break;
        default:
LABEL_200:
          v64 = 0;
          break;
      }
      BuyUnit.DstName.m_Name[0] = 0;
      BuyUnit.Data.m_Name[0] = 0;
      BuyUnit.GiftMsg.m_Msg[0] = 0;
      BuyUnit.Gid = 0i64;
      BuyUnit.TableIdx = TableIdx;
      BuyUnit.BuyTime = GetTime_t();
      BuyUnit.UseTime = GetTime_t();
      BuyUnit.Period = v64;
      BuyUnit.GetType = 0;
      if ( UseMileage == 1 )
        v50 = 2;
      else
        v50 = Hancoin == 0;
      BuyUnit.GoldType = v50;
      BuyUnit.Mito = v66;
      BuyUnit.Hancoin = Hancoin;
      BuyUnit.Mileage = Mileage;
      *(_DWORD *)&BuyUnit.Data.m_Name[0] = *(_DWORD *)&PlateName->m_Name[0];
      *(_DWORD *)&BuyUnit.Data.m_Name[2] = *(_DWORD *)&PlateName->m_Name[2];
      *(_DWORD *)&BuyUnit.Data.m_Name[4] = *(_DWORD *)&PlateName->m_Name[4];
      *(_DWORD *)&BuyUnit.Data.m_Name[6] = *(_DWORD *)&PlateName->m_Name[6];
      *(_DWORD *)&BuyUnit.Data.m_Name[8] = *(_DWORD *)&PlateName->m_Name[8];
      BuyUnit.State = 0;
      XiCsCharInfo::AddVisualItemBuyHistory(pCharInfo, &BuyUnit);
      v47 = pCharInfo->m_CharInfo.Name.m_Name;
      v48 = pCharInfo->m_CharInfo.Cid;
      v49 = BS_SingletonHeap<BS_GameDB,1>::GetInstance();
      BS_GameDB::InsertVisualItemBuyHistory(v49, v48, v47, &BuyUnit);
      if ( pItem && VisualItemCategory::IsEnableEquip_0(pItem->m_pItem->categoryIndex) == 1 )
      {
        pEquipedItem = XiCsCharInfo::GetEquipVisualItem(
                         pCharInfo,
                         pItem->m_MyVSItem.CarID,
                         pItem->m_pItem->categoryIndex);
        if ( pEquipedItem )
          XiCsCharInfo::UnEquipVisualItem(pCharInfo, pEquipedItem->m_MyVSItem.InvenIdx, pEquipedItem->m_MyVSItem.CarID);
        XiCsCharInfo::EquipVisualItem(pCharInfo, pItem->m_MyVSItem.InvenIdx, pItem->m_MyVSItem.CarID);
      }
      PacketSend::Send_VSItemModList((BS_PacketDispatch *)&pGameDispatch->vfptr);
      PacketSend::Send_BuyVisualItem(
        (BS_PacketDispatch *)&pGameDispatch->vfptr,
        pItem,
        v64,
        v66,
        Hancoin,
        Money,
        Mileage);
      PacketSend::Send_BonusUpdate((BS_PacketDispatch *)&pGameDispatch->vfptr);
      PacketSend::Send_VisualUpdate((BS_PacketDispatch *)&pGameDispatch->vfptr, lpBuyVisualItem->CarID);
      PacketSend::Send_StatUpdate((BS_PacketDispatch *)&pGameDispatch->vfptr);
      PacketSend::Send_PartyEnChantUpdateAll((BS_PacketDispatch *)&pGameDispatch->vfptr);
    }
    return CmdLen;
  }
  switch ( PeriodIdx )
  {
    case 0u:
      BS_StrUtils::BS_WidetoMB::BS_WidetoMB(&ItemCode, 0, 0);
      break;
    case 1u:
      BS_StrUtils::BS_WidetoMB::BS_WidetoMB(&ItemCode, pVShopItemInfo->tszItemCode007, 0);
      break;
    case 2u:
      BS_StrUtils::BS_WidetoMB::BS_WidetoMB(&ItemCode, pVShopItemInfo->tszItemCode030, 0);
      break;
    case 3u:
      if ( pVShopItemInfo == (XiVShop::ITEM_INFO *)-488 )
        BS_StrUtils::BS_WidetoMB::BS_WidetoMB(&ItemCode, (const wchar_t *)0x20, 0);
      else
        BS_StrUtils::BS_WidetoMB::BS_WidetoMB(&ItemCode, pVShopItemInfo->tszItemCode090, 0);
      break;
    case 4u:
      BS_StrUtils::BS_WidetoMB::BS_WidetoMB(&ItemCode, pVShopItemInfo->tszItemCode000, 0);
      break;
    default:
      BS_StrUtils::BS_WidetoMB::BS_WidetoMB(&ItemCode, 0, 0);
      break;
  }
  v110 = 1;
  BS_StrUtils::BS_WidetoMB::BS_WidetoMB(&ItemID, pVShopItemInfo->tszVisualItemId, 0);
  LOBYTE(v110) = 2;
  switch ( PeriodIdx )
  {
    case 0u:
      v55 = 0;
      break;
    case 1u:
      v55 = pVShopItemInfo->dwPeriod7Day;
      break;
    case 2u:
      v55 = pVShopItemInfo->dwPeriod30Day;
      break;
    case 3u:
      if ( pVShopItemInfo->dwPeriod90Day )
      {
        v55 = pVShopItemInfo->dwPeriod90Day;
      }
      else
      {
        if ( !pVShopItemInfo->dwPeriod365Day )
          goto LABEL_86;
        v55 = pVShopItemInfo->dwPeriod365Day;
      }
      break;
    case 4u:
      v55 = pVShopItemInfo->dwPeriod000Day;
      break;
    default:
LABEL_86:
      v55 = 0;
      break;
  }
  v21 = lpBuyVisualItem->CurCash;
  v22 = Hancoin;
  v23 = std::vector<XiVShop::ITEM_CATEGORY,std::allocator<XiVShop::ITEM_CATEGORY>>::const_iterator::operator*(&ItemID);
  v24 = std::vector<XiVShop::ITEM_CATEGORY,std::allocator<XiVShop::ITEM_CATEGORY>>::const_iterator::operator*(&ItemCode);
  if ( BillingProcess_Item(pCharInfo, v24, v23, v55, v22, v21) == 1 )
  {
    v25 = std::vector<XiVShop::ITEM_CATEGORY,std::allocator<XiVShop::ITEM_CATEGORY>>::const_iterator::operator*(&ItemCode);
    XiCsCharInfo::CalcHancoin(pCharInfo, Hancoin, "BUYITEM", "O", v25);
    LOBYTE(v110) = 1;
    if ( ItemID.m_pBuffer )
      operator delete[](ItemID.m_pBuffer);
    v110 = -1;
    if ( ItemCode.m_pBuffer )
      operator delete[](ItemCode.m_pBuffer);
    goto LABEL_98;
  }
  v26 = std::vector<XiVShop::ITEM_CATEGORY,std::allocator<XiVShop::ITEM_CATEGORY>>::const_iterator::operator*(&ItemCode);
  XiCsCharInfo::CalcHancoin(pCharInfo, Hancoin, "BUYITEM", "F", v26);
  v27 = lpDispatch->m_pSession;
  v28 = GTW("20094");
  PacketSend::Send_Error(v27, v28);
  v60 = CmdLen;
  LOBYTE(v110) = 1;
  if ( ItemID.m_pBuffer )
    operator delete[](ItemID.m_pBuffer);
  v110 = -1;
  if ( ItemCode.m_pBuffer )
    operator delete[](ItemCode.m_pBuffer);
  return v60;
}
*/