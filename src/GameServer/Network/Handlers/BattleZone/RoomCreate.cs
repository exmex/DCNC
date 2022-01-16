using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Util;
using Shared.Network.Packets.GameServer.BattleZone;
using Shared.Network.Packets.GameServer;


namespace GameServer.Network.Handlers
{
    public class RoomCreate
    {
        [Packet(Packetss.CmdRoomCreate)]
        public static void Handle(Packet packet)
        {

            /*__unaligned __declspec(align(1)) XiPvpUserInfo m_UserInfo;
            __unaligned __declspec(align(1)) int m_Result;
            __unaligned __declspec(align(1)) unsigned int m_RoomId;
            __unaligned __declspec(align(1)) unsigned int m_RoomLifeId;
            __int16 m_RoomType;
            unsigned __int16 m_MapId;
            unsigned __int16 m_MapFlag;*/
            var roomCreatePacket = new RoomCreatePacket(packet);
            //MapId, RoomType, PlayerCapacity, RoomName, RoomPass);
            var m_UserInfo = roomCreatePacket.m_UserInfo;
            var m_Result = 1;
            var m_RoomId = 101;
            var m_RoomLifeId = 0; //EXP Reward
            //var m_RoomType = roomCreatePacket.m_RoomType;
            var m_RoomType = 64;
            var m_MapId = roomCreatePacket.m_MapId;
            //var m_MapId = 2;
            var m_MapFlag = roomCreatePacket.m_MapFlag;
            //var m_MapFlag = 1;
            //var m_sync = 1;

            var ack = new Packet(Packetss.RoomCreateAck);
            var ack2 = new Packet(Packetss.RoomCreateAck);

            ack.Writer.Write(m_UserInfo);
            ack.Writer.Write(m_Result);
            ack.Writer.Write(m_RoomId);
            ack.Writer.Write(m_RoomLifeId);
            ack.Writer.Write(m_RoomType);
            ack.Writer.Write(m_MapId);
            ack.Writer.Write(m_MapFlag);

            //ack2.Writer.Write(m_sync);

            packet.Sender.Send(ack);
            //GameServer.Instance.Server.Broadcast(ack2);
            
        }

    }

    /*
     * //----- (005ABBE0) --------------------------------------------------------
char __cdecl BS_Room::CreateRoom(BS_SmartPtr<BS_Room> *pRoom, BS_GameDispatch *pGameDispatch, XiPvpUserInfo *UserInfo, unsigned __int16 MapId, unsigned __int16 MapFlag, unsigned int RoomType, unsigned __int16 PlayerCapacity, const wchar_t *RoomName, const wchar_t *RoomPass, int nPvpCh)
{
  BS_Session *lpSession; // ST15C_4@3
  wchar_t *v11; // eax@3
  BS_Session *v13; // ST158_4@5
  wchar_t *v14; // eax@5
  unsigned int v15; // ecx@8
  BS_GameDispatch *v16; // edx@8
  BS_Room *v17; // ST14C_4@12
  BS_FastRand *v18; // ST11C_4@18
  BS_FastRand *v19; // ST110_4@20
  BS_FastRand *v20; // ST104_4@21
  BS_Session *v21; // STC8_4@30
  wchar_t *v22; // eax@30
  BS_Room *v23; // ST98_4@39
  BS_Session *v24; // ST90_4@40
  wchar_t *v25; // eax@40
  unsigned int v26; // [sp+10h] [bp-164h]@13
  BS_GameDispatch *v27; // [sp+24h] [bp-150h]@41
  BS_GameDispatch *v28; // [sp+9Ch] [bp-D8h]@33
  std::pair<std::_Tree<std::_Tset_traits<BS_GameDispatch *,std::less<BS_GameDispatch *>,std::allocator<BS_GameDispatch *>,0> >::iterator,bool> result; // [sp+14Ch] [bp-28h]@41
  int v30; // [sp+154h] [bp-20h]@43
  BS_CriticalSection *v31; // [sp+158h] [bp-1Ch]@41
  int msg; // [sp+15Ch] [bp-18h]@35
  BS_CriticalSection *v33; // [sp+160h] [bp-14h]@33
  LapTimeInfo *info; // [sp+164h] [bp-10h]@15
  int v35; // [sp+170h] [bp-4h]@35

  if ( !(RoomType & 8) && !BS_Room::IsPermitted(nPvpCh, pGameDispatch, RoomType) )
  {
    lpSession = pGameDispatch->m_pSession;
    v11 = GTW("20040");
    PacketSend::Send_Error(lpSession, v11);
    return 0;
  }
  if ( pGameDispatch->m_pRoom.m_pObj )
  {
    v13 = pGameDispatch->m_pSession;
    v14 = GTW("20037");
    PacketSend::Send_Error(v13, v14);
    return 0;
  }
  if ( !pRoom->m_pObj )
  {
    PacketSend::Send_Error(pGameDispatch->m_pSession, &off_6C27C4);
    return 0;
  }
  v15 = UserInfo->Ip;
  v16 = pGameDispatch;
  *(_DWORD *)&pGameDispatch->m_pvpUserInfo.Level = *(_DWORD *)&UserInfo->Level;
  v16->m_pvpUserInfo.Ip = v15;
  if ( !RoomName || !*RoomName )
    RoomName = GTW((&s_roomNames)[4 * (++nIndex % 0xA)]);
  if ( RoomType == 64 )
  {
    pRoom->m_pObj->m_roomState = 8;
    v17 = pRoom->m_pObj;
    v17->m_waitStartTime = GetSystemTick();
    pRoom->m_pObj->m_LapTimeReadyTime = 0;
    std::_Tree<std::_Tset_traits<BS_GameDispatch *,std::less<BS_GameDispatch *>,std::allocator<BS_GameDispatch *>,0>>::clear((std::_Tree<std::_Tset_traits<BS_GameDispatch *,std::less<BS_GameDispatch *>,std::allocator<BS_GameDispatch *>,0> > *)&pRoom->m_pObj->m_completeSet.comp);
    std::_Tree<std::_Tset_traits<BS_GameDispatch *,std::less<BS_GameDispatch *>,std::allocator<BS_GameDispatch *>,0>>::clear((std::_Tree<std::_Tset_traits<BS_GameDispatch *,std::less<BS_GameDispatch *>,std::allocator<BS_GameDispatch *>,0> > *)&pRoom->m_pObj->m_waitSet.comp);
    v26 = g_LapTimeTable.m_laptime._Myfirst ? g_LapTimeTable.m_laptime._Mylast - g_LapTimeTable.m_laptime._Myfirst : 0;
    info = &g_LapTimeTable.m_laptime._Myfirst[nPvpCh % v26];
    if ( g_LapTimeTable.m_laptime._Myfirst[nPvpCh % v26].cityMap[1] )
    {
      if ( info->cityMap[2] )
      {
        if ( info->cityMap[3] )
        {
          v20 = BS_SingletonHeap<BS_FastRand,5>::GetInstance();
          MapId = (signed int)CGenRandMT::genrand_int31(&v20->m_randMT) % 4;
        }
        else
        {
          v19 = BS_SingletonHeap<BS_FastRand,5>::GetInstance();
          MapId = (signed int)CGenRandMT::genrand_int31(&v19->m_randMT) % 3;
        }
      }
      else
      {
        v18 = BS_SingletonHeap<BS_FastRand,5>::GetInstance();
        MapId = (signed int)CGenRandMT::genrand_int31(&v18->m_randMT) % 2;
      }
    }
    else
    {
      MapId = 0;
    }
    if ( BS_SingletonHeap<BS_Config,5>::GetInstance()->m_MapCheat > 0 )
      MapId = (BS_SingletonHeap<BS_Config,5>::GetInstance()->m_MapCheat - 1) % 4;
  }
  BS_Room::InitRoom(pRoom->m_pObj, MapId, RoomType, PlayerCapacity, RoomName, RoomPass);
  pRoom->m_pObj->m_MapFlag = MapFlag;
  pRoom->m_pObj->m_nPvpCh = nPvpCh;
  pGameDispatch->m_bReady = 0;
  if ( RoomType == 24 )
  {
    pRoom->m_pObj->vfptr->AddMember(pRoom->m_pObj, pGameDispatch, 1, 0);
    BS_Room::SendRoomState(pGameDispatch, pRoom->m_pObj, 0, 1);
  }
  else if ( RoomType == 8 )
  {
    pRoom->m_pObj->vfptr->AddMember(pRoom->m_pObj, pGameDispatch, 0, 0);
    BS_Room::SendRoomState(pGameDispatch, pRoom->m_pObj, 0, 1);
  }
  else if ( RoomType & 0x200 )
  {
    pRoom->m_pObj->m_MasterSerial = pGameDispatch->m_GameSessionId;
    if ( !BS_OBSRoom::AddOBS(&pRoom->m_pObj->m_OBSRoom, pGameDispatch) )
    {
      v21 = pGameDispatch->m_pSession;
      v22 = GTW("20160");
      PacketSend::Send_Error(v21, v22);
      return 0;
    }
    pRoom->m_pObj->vfptr->SendOBSState(pRoom->m_pObj, pGameDispatch);
  }
  else
  {
    pRoom->m_pObj->vfptr->AddMember(pRoom->m_pObj, pGameDispatch, -1, 0);
    BS_Room::SendRoomState(pGameDispatch, pRoom->m_pObj, 0, 1);
  }
  v28 = pGameDispatch;
  v33 = &pGameDispatch->m_lock;
  if ( pGameDispatch != (BS_GameDispatch *)-28 )
    EnterCriticalSection(&v33->m_csLock);
  v35 = 0;
  BS_MessageDispatch::GetMessageBuffer<BS_PktRoomCreateAck>(v28, (char **)&msg);
  v35 = 1;
  *(_WORD *)(msg + 22) = pRoom->m_pObj->m_RoomType;
  *(_WORD *)(msg + 24) = MapId;
  *(_WORD *)(msg + 26) = MapFlag;
  *(_DWORD *)(msg + 10) = 1;
  *(_DWORD *)(msg + 14) = pRoom->m_pObj->m_RoomId;
  v35 = -1;
  if ( v33 )
    LeaveCriticalSection(&v33->m_csLock);
  if ( RoomType == 64 )
  {
    if ( !pRoom->m_pObj->m_pAreaId )
    {
      v23 = pRoom->m_pObj;
      v23->m_pAreaId = BS_Room::GetFreeAreaId();
      if ( !pRoom->m_pObj->m_pAreaId )
      {
        v24 = pGameDispatch->m_pSession;
        v25 = GTW("20185");
        PacketSend::Send_Error(v24, v25);
        return 0;
      }
    }
    std::_Tree<std::_Tset_traits<BS_GameDispatch *,std::less<BS_GameDispatch *>,std::allocator<BS_GameDispatch *>,0>>::insert(
      (std::_Tree<std::_Tset_traits<BS_GameDispatch *,std::less<BS_GameDispatch *>,std::allocator<BS_GameDispatch *>,0> > *)&pRoom->m_pObj->m_waitSet.comp,
      &result,
      &pGameDispatch);
    v27 = pGameDispatch;
    v31 = &pGameDispatch->m_lock;
    if ( pGameDispatch != (BS_GameDispatch *)-28 )
      EnterCriticalSection(&v31->m_csLock);
    v35 = 3;
    BS_MessageDispatch::GetMessageBuffer<BS_PktLapTimeLoad>(v27, (char **)&v30);
    v35 = 4;
    *(_DWORD *)(v30 + 2) = pRoom->m_pObj->m_MapId;
    *(_DWORD *)(v30 + 6) = pRoom->m_pObj->m_pAreaId->Id;
    v35 = -1;
    if ( v31 )
      LeaveCriticalSection(&v31->m_csLock);
  }
  ((void (__thiscall *)(BS_GameDispatch *))pGameDispatch->vfptr[1].__vecDelDtor)(pGameDispatch);
  return 1;
}
     * 
     * 
     */

}
