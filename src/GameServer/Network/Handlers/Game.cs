using System;
using Shared.Models;
using Shared.Network;
using Shared.Objects;
using Shared.Util;

namespace GameServer.Network.Handlers
{
	public class Game
	{
		/*
		000000: 2F ED 00 00  / · · · 
		000000: 56 B3 00 00  V · · ·

		Wrong Packet Size. CMD(3918) CmdLen: : 6, AnalysisSize: 4
		*/
		[Packet(Packets.CmdUnknownSync)]
		public static void UnknownSync(Packet packet) // TODO: Figure out what this packet does...
		{
			packet.Reader.ReadInt32(); // always the same in session
			// hide sync packets for now

			var ack = new Packet(Packets.CmdUnknownSync+1);
			ack.Writer.Write((short)0);
		}

		[Packet(Packets.CmdMyCityPosition)] // TODO: Actual position and not just dummies
		public static void MyCityPosition(Packet packet)
		{
			//Console.WriteLine(packet.Reader.ReadUInt32());
			// SHORT CHANNELID
			// -> Gate ID!
			var ack = new Packet(Packets.MyPositionAck);

			//ack.Writer.Write(256);
			ack.Writer.Write(packet.Sender.User.ActiveCharacter.City); // City ID
			ack.Writer.Write(1); // Channel ID
			ack.Writer.Write(packet.Sender.User.ActiveCharacter.PositionX); // x
			ack.Writer.Write(packet.Sender.User.ActiveCharacter.PositionY); // y
			ack.Writer.Write(packet.Sender.User.ActiveCharacter.PositionZ); // z
			ack.Writer.Write(packet.Sender.User.ActiveCharacter.Rotation); // w
			ack.Writer.Write(packet.Sender.User.ActiveCharacter.posState); // PosState
			/*int m_CityId;
			int m_ChannelId;
			XiVec4 m_Pos;
			int m_PositionState;*/

			packet.Sender.Send(ack);
		}


		/*
		000000: 00 00 00 00 00 00 00 00 00 00 00 00 4A 70 AB 42  · · · · · · · · · · · · J p · B
		000016: 00 00 00 00 01 00 00 00 01 00 00 00  · · · · · · · · · · · ·
		*/
		[Packet(Packets.CmdSaveCarPos)]
		public static void SaveCarPos(Packet packet)
		{
			var channelId = packet.Reader.ReadInt32();
			var x = packet.Reader.ReadSingle();
			var y = packet.Reader.ReadSingle();
			var z = packet.Reader.ReadSingle();
			var w = packet.Reader.ReadSingle();
			var cityId = packet.Reader.ReadInt32();
			var posState = packet.Reader.ReadInt32();

			CharacterModel.UpdatePosition(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId, channelId, x, y, z, w, cityId, posState);
		}

		[Packet(Packets.CmdUpdateQuickSlot)]
		public static void UpdateQuickSlot(Packet packet)
		{
			// TODO: actually update the quickslots.
			var slot1 = packet.Reader.ReadUInt32();
			var slot2 = packet.Reader.ReadUInt32();
		}

		[Packet(Packets.CmdGetDateTime)]
		public static void GetDateTime(Packet packet)
		{
			var unixTimeNow = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
			var now = DateTime.UtcNow.ToLocalTime();

			var globalTime = packet.Reader.ReadUInt32(); // GlobalTime
			var localTime = packet.Reader.ReadUInt32(); // LocalTime
			var action = packet.Reader.ReadUInt32(); // Action

			var ack = new Packet(Packets.GetDateTimeAck);
			ack.Writer.Write(action);
			ack.Writer.Write(globalTime);
			ack.Writer.Write(localTime);
			ack.Writer.Write((int)unixTimeNow.TotalSeconds);
			ack.Writer.Write(0); // ServerTickTime
			ack.Writer.Write(0); // ServerTick
			ack.Writer.Write((short)now.DayOfYear);
			ack.Writer.Write((short)now.Month);
			ack.Writer.Write((short)now.Day);
			ack.Writer.Write((short)now.DayOfWeek);
			ack.Writer.Write((byte)now.Hour);
			ack.Writer.Write((byte)now.Minute);
			ack.Writer.Write((byte)now.Second);

			packet.Sender.Send(ack);

			/*
			  *(_DWORD *)(msg + 2) = lpMsg->Action;
			  *(_DWORD *)(msg + 6) = lpMsg->GlobalTime;
			  *(float *)(msg + 10) = lpMsg->LocalTime;
			  *(_DWORD *)(msg + 14) = now;
			  * *(_DWORD *)(msg + 18) = GetServerTickTime();
			  *(_DWORD *)(msg + 22) = GetServerTick();
			  *(_WORD *)(msg + 26) = ptm->tm_yday;
			  *(_WORD *)(msg + 28) = ptm->tm_mon;
			  *(_WORD *)(msg + 30) = ptm->tm_mday;
			  *(_WORD *)(msg + 32) = ptm->tm_wday;
			  *(_BYTE *)(msg + 34) = ptm->tm_hour;
			  *(_BYTE *)(msg + 35) = ptm->tm_min;
			  *(_BYTE *)(msg + 36) = ptm->tm_sec;
			 */
		}

		

		/*
		000000: 01 00 00 00 01 00 00 00 00 00 00 00 00 00 80 3F  · · · · · · · · · · · · · · · ?
		000016: 00 00 00 00  · · · · 
		*/
		[Packet(Packets.CmdFuelChargeReq)]
		public static void FuelChargeReq(Packet packet) // TODO: Send actual data
		{
			uint CarId = packet.Reader.ReadUInt32();
			long Pay = packet.Reader.ReadInt64();
			float fuel = packet.Reader.ReadSingle();

			/*
			  unsigned int CarId;
			  __unaligned __declspec(align(1)) __int64 Pay;
			  float DeltaFuel;
			  __int64 Gold;
			  float Fuel;
			  float UnitPrice;
			  float SaleUnitPrice;
			  float FuelCapacity;
			  float FuelEfficiency;
			  int SaleFlag;
			*/

			var ack = new Packet(Packets.FuelChargeReqAck);
			ack.Writer.Write(CarId);
			ack.Writer.Write(Pay);
			ack.Writer.Write(fuel);
			ack.Writer.Write(packet.Sender.User.ActiveCharacter.MitoMoney-Pay);
			ack.Writer.Write(packet.Sender.User.ActiveCar.Mitron+fuel);
			ack.Writer.Write(20.0f); // Mito Price per liter
			ack.Writer.Write(15.0f); // Discounted Price per liter (Normal channel, major channel is free?)
			ack.Writer.Write(packet.Sender.User.ActiveCar.MitronCapacity);
			ack.Writer.Write(packet.Sender.User.ActiveCar.MitronEfficiency);

			ack.Writer.Write(0); // 1 = Item Discount 50%
			ack.Writer.Write(new byte[2]); // Not sure.
			packet.Sender.Send(ack);
			/*
			SaleFlag = 0;
			if ( pGame->m_pCharInfo->m_bHalfMitronCharge )
			{
			   fSaleRate = fSaleRate - 0.5;
				v7 = SaleFlag;
				LOBYTE(v7) = SaleFlag | 1;
				SaleFlag = v7;
			}
			*/
		}

		[Packet(Packets.CmdChatMsg)]
		public static void ChatMessage(Packet packet)
		{
			string type = packet.Reader.ReadUnicodeStatic(10);
			bool green = packet.Reader.ReadUInt32() == 0xFF00FF00; // ignore this, use packet.Sender.Player.User.Status
			string message = packet.Reader.ReadUnicodePrefixed();

			string sender = packet.Sender.User.ActiveCharacter.Name;

			Log.Debug($"({type}) <{sender}> {message}");

			var ack = new Packet(Packets.ChatMsgAck);
			ack.Writer.WriteUnicodeStatic(type, 10);
			ack.Writer.WriteUnicodeStatic(sender, 18);
			ack.Writer.WriteUnicode(message);

			switch (type)
			{
				case "room":
					GameServer.Instance.Server.Broadcast(ack); // TODO: broadcast only to users in same area
					break;

				case "channel":
					GameServer.Instance.Server.Broadcast(ack);
					break;

				case "party":
					GameServer.Instance.Server.Broadcast(ack); // TODO: broadcast only to users in same party
					break;

				case "team":
					GameServer.Instance.Server.Broadcast(ack); // TODO: broadcast only to users in same crew
					break;

				default:
					Log.Error("Undefined chat message type.");
					break;
			}
		}

		[Packet(Packets.CmdMyTeamInfo)]
		public static void MyTeamInfo(Packet packet)
		{
			uint act = packet.Reader.ReadUInt32(); // nAct?

			var ack = new Packet(Packets.MyTeamInfoAck);

			ack.Writer.Write(act); // Action (1003, 1004, 1031, 1034)
            
            /* After the action id, it seems the rest until byte 16 is ignored? */
            ack.Writer.Write(packet.Sender.User.ActiveCharacterId); // Char ID
            ack.Writer.Write(0); // Rank
            /* After the action id, it seems the rest above is ignored*/

            // This must be 664 bytes long starting at byte 18
			packet.Sender.User.ActiveTeam.Serialize(ack.Writer);
			ack.Writer.Write((ushort)0); // Age?
			packet.Sender.Send(ack);
			/*
			  unsigned int m_Act;
			  __int64 m_Cid;
			  int m_TeamRank;
			  XiStrTeamInfo m_TeamInfo;
			  unsigned __int16 m_Age;
			*/
		}

		[Packet(Packets.CmdGameCharInfo)]
		public static void GameCharInfo(Packet packet) // TODO: Send data correspoding to the charname, not user
		{
			/*
			[Debug] - GameCharInfo: 000000: 41 00 64 00 6D 00 69 00 6E 00 69 00 73 00 74 00  A · d · m · i · n · i · s · t ·
			000016: 72 00 61 00 74 00 6F 00 72 00 00 00 0A 00 06 00  r · a · t · o · r · · · · · · ·
			000032: 0D 25 33 65 63 65 69 76 65 64 00 00 00 00  · % 3 e c e i v e d · · · ·

			[Info] - Received GameCharInfo (id 660, 0x294) on 11021.

			Wrong Packet Size. CMD(661) CmdLen: : 1177, AnalysisSize: 831
			// We're missing 346 bytes of data.
			*/
			var charName = packet.Reader.ReadUnicodeStatic(21);
			var ack = new Packet(Packets.GameCharInfoAck);
			packet.Sender.User.ActiveCharacter.Serialize(ack.Writer);
			packet.Sender.User.ActiveCar.Serialize(ack.Writer);
			var sinfo = new StatInfo();
			sinfo.Serialize(ack.Writer);
			packet.Sender.User.ActiveTeam.Serialize(ack.Writer);

			ack.Writer.Write((uint)0); // Serial
			ack.Writer.Write('A'); // LocType
			ack.Writer.Write('A'); // ChId
			ack.Writer.Write((ushort)1); // LocId

			packet.Sender.Send(ack);
		}

		[Packet(Packets.CmdChaseRequest)]
		public static void ChaseRequest(Packet packet)
		{
			/*
			[Debug] - ChaseRequest: 000000: 01 9A 45 91 45 66 4A AA 45 A6 52 E7 40 00 00 00  · · E · E f J · E · R · @ · · ·
			000016: 00  ·

			[Info] - Received ChaseRequest (id 189, 0xBD) on 11021.
			*/

			var bNow = packet.Reader.ReadBoolean();
			var posX = packet.Reader.ReadSingle(); // X
			var posY = packet.Reader.ReadSingle(); // Y
			var posZ = packet.Reader.ReadSingle(); // Z
			var rot = packet.Reader.ReadSingle(); // W

			//Wrong Packet Size. CMD(186) CmdLen: : 252, AnalysisSize: 250
			var ack = new Packet(Packets.ChasePropose);
			ack.Writer.Write((ushort)0);
			ack.Writer.Write(posX); // Start X
			ack.Writer.Write(posY); // Start Y
			ack.Writer.Write(posZ); // Start Z
			ack.Writer.Write(rot); // Start W

			ack.Writer.Write(posX); // End X
			ack.Writer.Write(posY); // End Y
			ack.Writer.Write(posZ); // End Z

			ack.Writer.Write(0); // CourseId
			ack.Writer.Write(2 - (bNow ? 1 : 0)); // Type?
			ack.Writer.WriteUnicodeStatic("test", 100);

			ack.Writer.Write(1); // HUV first level
			ack.Writer.Write(10001); // HUV first Id
			ack.Writer.Write(new byte[2]); // Not sure.
			packet.Sender.Send(ack);
		}

		[Packet(Packets.CmdInstantStart)]
		public static void InstantStart(Packet packet)
		{
			var tableIdx = packet.Reader.ReadUInt32();

			var ack = new Packet(Packets.InstantStartAck);
			ack.Writer.Write(tableIdx);
			ack.Writer.Write(new byte[3]); // Missing?
			packet.Sender.Send(ack);
		}
		
		[Packet(Packets.CmdInstantGoalPlace)]
		public static void InstantGoalPlace(Packet packet)
		{
			var tableIdx = packet.Reader.ReadUInt32();
			var placeIdx = packet.Reader.ReadUInt32();
			
			var ack = new Packet(Packets.CmdInstantGoalPlace+1);
			ack.Writer.Write(tableIdx);
			ack.Writer.Write(placeIdx);
			ack.Writer.Write(0); // EXP??
			ack.Writer.Write(new byte[28]);
			packet.Sender.Send(ack);
		}

		[Packet(Packets.CmdInstantGiveUp)]
		public static void InstantGiveUp(Packet packet)
		{
			var tableIdx = packet.Reader.ReadUInt32();

			var ack = new Packet(Packets.InstantGiveUpAck);
			ack.Writer.Write(tableIdx);
			packet.Sender.Send(ack);
		}
		
		//000000: 01 00 00 00  · · · ·
		[Packet(Packets.CmdQuestStart)]
		public static void QuestStart(Packet packet)
		{
			var tableIdx = packet.Reader.ReadUInt32();

		    QuestModel.Add(GameServer.Instance.Database.Connection, new Quest
		    {
		        CharacterId = packet.Sender.User.ActiveCharacterId,
                CharacterName = packet.Sender.User.ActiveCharacter.Name,
                FailNum = 0,
                PlaceIdx = 0,
                QuestId = tableIdx,
                ServerId = 0,
                State = 0,
		    });

            var ack = new Packet(Packets.CmdQuestStart+1);
			ack.Writer.Write(tableIdx);
			ack.Writer.Write(0); // Fail num maybe?
			packet.Sender.Send(ack);
		}
		
		[Packet(Packets.CmdQuestGiveUp)]
		public static void QuestGiveup(Packet packet)
		{
			var tableIdx = packet.Reader.ReadUInt32();

            QuestModel.Update(GameServer.Instance.Database.Connection, 0, packet.Sender.User.ActiveCharacterId, tableIdx, 4);

            var ack = new Packet(Packets.CmdQuestGiveUp+1);
			ack.Writer.Write(tableIdx);
			ack.Writer.Write((byte)0);
			packet.Sender.Send(ack);
		}
		
		//000000: 01 00 00 00  · · · ·
		[Packet(Packets.CmdQuestGoalPlace)]
		public static void QuestGoalPlace(Packet packet)
		{
		}
		
		//000000: 01 00 00 00 14 43 8B 42 4E 9E D0 FE  · · · · · C · B N · · ·
		[Packet(Packets.CmdQuestComplete)]
		public static void QuestComplete(Packet packet)
		{
			var tableIdx = packet.Reader.ReadUInt32();

            QuestModel.Update(GameServer.Instance.Database.Connection, 0, packet.Sender.User.ActiveCharacterId, tableIdx, 1);
			
			var ack = new Packet(Packets.QuestCompleteAck);
			ack.Writer.Write(tableIdx);
			packet.Sender.Send(ack);
		}

	    [Packet(Packets.CmdQuestReward)]
	    public static void QuestReward(Packet packet)
	    {
            var tableIdx = packet.Reader.ReadUInt32();

	        Quest quest = QuestModel.RetrieveOne(GameServer.Instance.Database.Connection, 0, packet.Sender.User.ActiveCharacterId,
	            tableIdx);
            QuestModel.Update(GameServer.Instance.Database.Connection, 0, packet.Sender.User.ActiveCharacterId, tableIdx, 2);
            if (quest == null)
	        {
                packet.Sender.Error("Quest was not started!");
	            return;
	        }
	        if (quest.State != 1)
	        {
                packet.Sender.Error("Quest not finished!");
                return;
            }

	        if (!GameServer.QuestTable.ContainsKey(tableIdx))
	        {
	            packet.Sender.Error("Quest reward not found.");
	            return;
	        }
	        XiStrQuest questReward = GameServer.QuestTable[tableIdx];
            packet.Sender.User.ActiveCharacter.CurExp += questReward.RewardExp;
            // TODO: Check if user has leveled up.

            // TODO: Load quest reward information here, and send it.

            var ack = new Packet(Packets.QuestRewardAck);
            ack.Writer.Write((uint)questReward.QuestIdN); // TableIdx
            ack.Writer.Write((uint)questReward.RewardExp); // GetExp
            ack.Writer.Write((uint)questReward.RewardMoney); // GetMoney
            ack.Writer.Write((ulong)packet.Sender.User.ActiveCharacter.CurExp); // ExpInfo Current
            ack.Writer.Write((ulong)packet.Sender.User.ActiveCharacter.NextExp); // ExpInfo Next
            ack.Writer.Write((ulong)packet.Sender.User.ActiveCharacter.BaseExp); // ExpInfo Base
            ack.Writer.Write((ushort)packet.Sender.User.ActiveCharacter.Level); // Level
            ack.Writer.Write((ushort)0); // ItemNum
            ack.Writer.Write((uint)0); // RewardItem
            ack.Writer.Write((uint)0); // RewardItem
            ack.Writer.Write((uint)0); // RewardItem

            packet.Sender.Send(ack);

            /*
            if ( pStrQuest->Item01Ptr || pStrQuest->Item02Ptr || pStrQuest->Item03Ptr )
              PacketSend::Send_ItemModList((BS_PacketDispatch *)&pGameDispatch->vfptr);
            */
            
	        CharacterModel.UpdateExp(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacter);
	    }

        //signed __int16 __usercall sub_52CAB0@<ax>(int a1@<ebp>, double a2@<st0>, int a3, int a4)
        [Packet(Packets.CmdCityLeaveCheck)]
		public static void CityLeaveCheck(Packet packet)
        {
            int cityId = packet.Reader.ReadInt32(); // CityId?
            string post1 = packet.Reader.ReadAsciiStatic(255); // Gate?
            string post2 = packet.Reader.ReadAsciiStatic(255); // Gate?

            Console.WriteLine(post2);


			var ack = new Packet(Packets.CityLeaveCheckAck);
		    ack.Writer.Write(1); // Result???
		    ack.Writer.Write(cityId);
            /* 0-Moon Palace, 1=Koinonia, 2=Cras, 3=Oros, 4=Taipei, 5=NeoOros, else szPassword? */
            ack.Writer.WriteAsciiStatic(post1, 255);
            ack.Writer.WriteAsciiStatic(post2, 255);
            packet.Sender.Send(ack);

            /*
             * Used bytes are:
             * 2 - 6 -> Result? // int?
             * 6 - 10 -> CityId? // int?
             * 10 - ?? -> Some kind of string, maybe Gate? // 255?
             * 265 - ?? -> Some kind of string, maybe Gate? // 255?
             */
        }

        [Packet(Packets.CmdBuyCar)]
		public static void BuyCar(Packet packet)
		{
			var charName = packet.Reader.ReadUnicodeStatic(21); // Possibly 21?
			Console.WriteLine(charName);
			
			uint CarType = packet.Reader.ReadUInt32();
			Console.WriteLine(CarType);
			uint Bumper = packet.Reader.ReadUInt16();
			Console.WriteLine(Bumper);
			uint Color = packet.Reader.ReadUInt16();
			Console.WriteLine(Color);
			
			// TODO: Buy car still has not the correct structure.
			var ack = new Packet(Packets.BuyCarAck);
			ack.Writer.Write(1); // ID
			ack.Writer.Write(CarType);
			ack.Writer.Write(0); // BaseColor?
			ack.Writer.Write(0); // Grade?
			ack.Writer.Write(0); // SlotType?
			ack.Writer.Write(0); // AuctionCnt?
			ack.Writer.Write(100.0f); // Mitron?
			ack.Writer.Write(0.0f); // Kmh?
            ack.Writer.Write(Color);
            ack.Writer.Write(0.0f); // Mitron cap?
			ack.Writer.Write(0.0f); // Mitron eff?
			ack.Writer.Write(false); // AuctionOn
			ack.Writer.Write(0); // ?????
			ack.Writer.Write((short)0); // ?????
            ack.Writer.Write(10000); // Price
            //ack.Writer.Write(Bumper);
            packet.Sender.Send(ack);
			/*PacketSend::Send_StatUpdate((BS_PacketDispatch *)&pGameDispatch->vfptr);
			  PacketSend::Send_PartyEnChantUpdateAll((BS_PacketDispatch *)&pGameDispatch->vfptr);
			  PacketSend::Send_ItemModList((BS_PacketDispatch *)&pGameDispatch->vfptr);
			  PacketSend::Send_VSItemModList((BS_PacketDispatch *)&pGameDispatch->vfptr);
			  PacketSend::Send_VisualUpdate((BS_PacketDispatch *)&pGameDispatch->vfptr, 0);
		  
			  qmemcpy(&lpAckPkt->CarInfo, pCharInfo->m_pCurCarInfo, 0x2Cu);
			v28[44] = v27->m_CarInfo.AuctionOn;
			lpAckPkt->Gold = Price;
			*/
		}
		
		[Packet(Packets.CmdGetMyHancoinThread)]
		public static void GetMyHancoin(Packet packet)
		{
			var ack = new Packet(Packets.GetMyHancoinAck);
			ack.Writer.Write(10000); // Hancoins?
			ack.Writer.Write(100); // Mileage?
			packet.Sender.Send(ack);
		}
		
		[Packet(Packets.CmdCheckStat)]
		public static void CheckStat(Packet packet)
		{
			var ack = new Packet(Packets.StatUpdateAck);
			ack.Writer.Write(0); // Speed (Car) Testvalue:100 -> http://i.imgur.com/AndRGwK.png
			ack.Writer.Write(0); // Durability (Car) Testvalue:100 -> http://i.imgur.com/zuaxZu5.png
			ack.Writer.Write(0); // Acceleration (Car) Testvalue:100 -> http://i.imgur.com/97UkLkj.png
			ack.Writer.Write(0); // Boost (Car) Testvalue:100 -> http://i.imgur.com/FQ9EYVO.png
			ack.Writer.Write(0); // Speed (Parts) Testvalue:300 -> http://i.imgur.com/FQ9EYVO.png
			ack.Writer.Write(0); // Durability (Parts) Testvalue:400 -> http://i.imgur.com/FQ9EYVO.png
			ack.Writer.Write(0); // Acceleration (Parts) Testvalue:500 -> http://i.imgur.com/FQ9EYVO.png
			ack.Writer.Write(0); // Boost (Parts) Testvalue:600 -> http://i.imgur.com/FQ9EYVO.png
						
			ack.Writer.Write(0); // Speed (User)
			ack.Writer.Write(0); // Durability (User)
			ack.Writer.Write(0); // Acceleration (User)
			ack.Writer.Write(0); // Boost (User)
			ack.Writer.Write(0); // Speed (User) WTF?
			ack.Writer.Write(0); // Durability (User) WTF?
			ack.Writer.Write(0); // Acceleration (User) WTF?
			ack.Writer.Write(0); // Boost (User) WTF?
			ack.Writer.Write(0); // Char Speed
			ack.Writer.Write(0); // Char Durability
			ack.Writer.Write(0); // Char Acceleration
			ack.Writer.Write(0); // Char Boost
			ack.Writer.Write(0); // int ItemUseSpeed;
			ack.Writer.Write(0); // int ItemUseCrash;
			ack.Writer.Write(0); // int ItemUseAccel;
			ack.Writer.Write(0); // int ItemUseBoost;
			ack.Writer.Write(0); // Unknown
			ack.Writer.Write(0); // Unknown
			ack.Writer.Write(0); // Unknown
			ack.Writer.Write(0); // Unknown
			ack.Writer.Write(0); // Vehicle Speed Testvalue:100 -> http://i.imgur.com/3GV9enQ.png
			ack.Writer.Write(0); // Vehicle Durability Testvalue:200 -> http://i.imgur.com/3GV9enQ.png
			ack.Writer.Write(0); // Vehicle Acceleration Testvalue:300 -> http://i.imgur.com/3GV9enQ.png
			ack.Writer.Write(0); // Vehicle Boost Testvalue:400 -> http://i.imgur.com/3GV9enQ.png
			ack.Writer.Write(0); // Unknown
			ack.Writer.Write(0); // Unknown
			ack.Writer.Write(0); // Unknown
			ack.Writer.Write(0); // Unknown
			ack.Writer.Write(0); // Unknown
			ack.Writer.Write(0); // Unknown
			ack.Writer.Write((short)0); // Unknown
			packet.Sender.Send(ack);
			/*
			Send_StatUpdate:
				XiStrStatInfo StatInfo;
				XiStrEnChantBonus EnChantBonus;
				
			struct XiStrEnChantBonus
{
  int Speed;
  int Crash;
  int Accel;
  int Boost;
  float Drop;
  float Exp;
  float MitronCapacity;
  float MitronEfficiency;
};

struct XiStrStatInfo
{
  int BasedSpeed;
  int BasedCrash;
  int BasedAccel;
  int BasedBoost;
  int EquipSpeed;
  int EquipCrash;
  int EquipAccel;
  int EquipBoost;
  int CharSpeed;
  int CharCrash;
  int CharAccel;
  int CharBoost;
  int ItemUseSpeed;
  int ItemUseCrash;
  int ItemUseAccel;
  int ItemUseBoost;
  int TotalSpeed;
  int TotalCrash;
  int TotalAccel;
  int TotalBoost;
};
			
			XiCsCharInfo::StatUpdate(pCharInfo);
			PacketSend::Send_StatUpdate(lpDispatch);
			PacketSend::Send_PartyEnChantUpdateAll(lpDispatch); // TODO
			*/
		}

	    [Packet(Packets.CmdBuyItem)]
	    public static void BuyItem(Packet packet)
	    {
            int itemID = packet.Reader.ReadInt16();
            int unknown = packet.Reader.ReadInt16();
            int quantity = packet.Reader.ReadInt16();

            var ack = new Packet(Packets.BuyItemAck);
            ack.Writer.Write(itemID);
            ack.Writer.Write(quantity);
            ack.Writer.Write(0);

            packet.Sender.Send(ack);

	        ack = new Packet(Packets.ItemModListAck);
            ack.Writer.Write(1);

            ack.Writer.Write((uint)4); // CarID
            ack.Writer.Write((ushort)1); // State
            ack.Writer.Write((ushort)1); // Slot
            ack.Writer.Write((uint)1); // StateVar
            ack.Writer.Write(quantity); // StackNum
            ack.Writer.Write(0); // Random

            ack.Writer.Write((uint)0); // AssistA
            ack.Writer.Write((uint)0); // AssistB
            ack.Writer.Write((uint)0); // Box
            ack.Writer.Write((uint)0); // Belonging
            ack.Writer.Write(0); // Upgrade
            ack.Writer.Write(0); // UpgradePoint
            ack.Writer.Write((uint)0); // ExpireTick
            ack.Writer.Write((uint)itemID); // TableIdx
            ack.Writer.Write((uint)0); // InvenIdx

            ack.Writer.Write(0); // State
            packet.Sender.Send(ack);

            //BS_PktItemDataList

            //PacketSend::Send_ItemModList((BS_PacketDispatch *)&pGameDispatch->vfptr);
        }

        [Packet(Packets.CmdBuyVisualItemThread)]
		public static void BuyItemVisual(Packet packet)
		{
			var tableIdx = packet.Reader.ReadUInt32();
			var carId = packet.Reader.ReadUInt32();
			var plateName = packet.Reader.ReadUnicodeStatic(10);
			packet.Reader.ReadInt32(); // Unknown
			packet.Reader.ReadInt32(); // Unknown
			packet.Reader.ReadInt32(); // Unknown
			packet.Reader.ReadInt32(); // Unknown
			packet.Reader.ReadInt32(); // Unknown
			// 20 bytes
			
			var periodIdx = packet.Reader.ReadUInt32();
			
			packet.Reader.ReadInt16(); // Unknown / 2 bytes
			packet.Reader.ReadInt64(); // curCash
			
			packet.Reader.ReadInt32(); // Unknown
			
			var ack = new Packet(Packets.CmdBuyVisualItemThread+1);
			ack.Writer.Write(0); // Type?
			ack.Writer.Write(tableIdx);
			ack.Writer.Write(carId);
			ack.Writer.Write(0); // Inventory Id (Slot?)
			ack.Writer.Write(0); // Period (1 = 7d, 2 = 30d, 4 = 00?, 5 = infinite)
			ack.Writer.Write(0); // Mito
			ack.Writer.Write(0); // Hancoin
			ack.Writer.Write(0); // BonusMito
			ack.Writer.Write(0); // Mileage
			
			packet.Sender.Send(ack);
			
			var ack2 = new Packet(Packets.RoomNotifyChangeAck); // TODO: Wrong Packet Size. CMD(467) CmdLen: : 240, AnalysisSize: 62
			ack2.Writer.Write((ushort)0); // Serial?
			ack2.Writer.Write((ushort)0); // Age?
			
			ack2.Writer.Write((ushort)0); // CarAttr Sort
			ack2.Writer.Write((ushort)0); // CarAttr Body
			ack2.Writer.Write(new char[4]); // CarAttr Color
			ack2.Writer.Write(0); // CarAttr lvalSortBody
			ack2.Writer.Write(0); // CarAttr lvalColor
			ack2.Writer.Write((long)0); // CarAttr llval
			
			ack2.Writer.WriteUnicodeStatic("Gigatoni", 21); // PlayerInfo Cname
			ack2.Writer.Write((ushort)5); // PlayerInfo serial
			ack2.Writer.Write((ushort)0); // PlayerInfo age
			ack2.Writer.Write((long)4); // PlayerInfo Cid
			ack2.Writer.Write((ushort)1); // PlayerInfo Level
			ack2.Writer.Write((uint)0); // PlayerInfo Exp
			ack2.Writer.Write((long)0); // PlayerInfo TeamId
			ack2.Writer.Write((long)0); // PlayerInfo TeamMarkId
			ack2.Writer.WriteUnicodeStatic("Staff", 14); // PlayerInfo TeamName
			ack2.Writer.Write((ushort)1); // PlayerInfo teamNLevel
			
			ack2.Writer.Write((ushort)1); // VisualItem Neon
			ack2.Writer.Write((ushort)1); // VisualItem Plate
			ack2.Writer.Write((ushort)1); // VisualItem Decal
			ack2.Writer.Write((ushort)1); // VisualItem DecalColor
			ack2.Writer.Write((ushort)1); // VisualItem AeroBumper
			ack2.Writer.Write((ushort)1); // VisualItem AeroIntercooler
			ack2.Writer.Write((ushort)1); // VisualItem AeroSet
			ack2.Writer.Write((ushort)1); // VisualItem MufflerFlame
			ack2.Writer.Write((ushort)1); // VisualItem Wheel
			ack2.Writer.Write((ushort)1); // VisualItem Spoiler
			
			ack2.Writer.Write((ushort)0); // VisualItem Reserved?
			ack2.Writer.Write((ushort)0); // VisualItem Reserved?
			ack2.Writer.Write((ushort)0); // VisualItem Reserved?
			ack2.Writer.Write((ushort)0); // VisualItem Reserved?
			ack2.Writer.Write((ushort)0); // VisualItem Reserved?
			ack2.Writer.Write((ushort)0); // VisualItem Reserved?
			
			ack2.Writer.WriteUnicodeStatic("HELLO", 9); // VisualItem PlateString
			
			ack2.Writer.Write((float)100.0f); // PlayerInfo UseItem
			
			/*
struct XiVisualItem
{
  __int16 Neon;
  __int16 Plate;
  __int16 Decal;
  __int16 DecalColor;
  __int16 AeroBumper;
  __int16 AeroIntercooler;
  __int16 AeroSet;
  __int16 MufflerFlame;
  __int16 Wheel;
  __int16 Spoiler;
  __int16 Reserve[6];
  wchar_t PlateString[9];
};
			*/
			
			packet.Sender.Send(ack2);
			
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
		
		/*BuyHistoryList: int32 pageNumber, int32 unknown, int32 tab (1=purchase history, 2=sent gift, 3=received gift)*/
		/* Mito gotcha play: packet number 3500*/
	}
}
