using System;
using Shared.Models;
using Shared.Network;
using Shared.Network.AreaServer.GameServer.Incoming;
using Shared.Network.AuthServer;
using Shared.Network.GameServer;
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

            var ack = new Packet(Packets.UnknownSyncAck);
            ack.Writer.Write((short) 0);
        }

        [Packet(Packets.CmdMyCityPosition)]
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

            // Rice sends an extra byte for some reason??
            //ack.Writer.Write((byte)0);

            packet.Sender.Send(ack);
        }

        /*
        000000: 00 00 00 00 00 00 00 00 00 00 00 00 4A 70 AB 42  · · · · · · · · · · · · J p · B
        000016: 00 00 00 00 01 00 00 00 01 00 00 00  · · · · · · · · · · · ·
        */
        [Packet(Packets.CmdSaveCarPos)]
        public static void SaveCarPos(Packet packet)
        {
            var saveCar = new SaveCarPosPacket(packet);

            CharacterModel.UpdatePosition(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId,
                saveCar.ChannelId, saveCar.X, saveCar.Y, saveCar.Z, saveCar.W, saveCar.CityId, saveCar.PosState);
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

            var getDateTimePacket = new GetDateTimePacket(packet);

            var ack = new GetDateTimePacketAnswer
            {
                Action = getDateTimePacket.Action,
                GlobalTime = getDateTimePacket.GlobalTime,
                LocalTime = getDateTimePacket.LocalTime,
                TotalSeconds = (int) unixTimeNow.TotalSeconds,
                ServerTickTime = 0,
                ServerTick = 0,
                DayOfYear = (short) now.DayOfYear,
                Month = (short) now.Month,
                Day = (short) now.Day,
                DayOfWeek = (short) now.DayOfWeek,
                Hour = (byte) now.Hour,
                Minute = (byte) now.Minute,
                Second = (byte) now.Second
            };

            packet.Sender.Send(ack.CreatePacket());

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
            var fuelChargeReqPacket = new FuelChargeReqPacket(packet);

            var ack = new FuelChargeReqAnswer
            {
                CarId = fuelChargeReqPacket.CarId,
                Pay = fuelChargeReqPacket.Pay,
                Fuel = fuelChargeReqPacket.Fuel,
                Gold = packet.Sender.User.ActiveCharacter.MitoMoney - fuelChargeReqPacket.Pay,
                DeltaFuel = packet.Sender.User.ActiveCar.Mitron + fuelChargeReqPacket.Fuel,
                SaleUnitPrice = 20.0f,
                DiscountedSaleUnitPrice = 15.0f,
                FuelCapacity = packet.Sender.User.ActiveCar.MitronCapacity,
                FuelEfficiency = packet.Sender.User.ActiveCar.MitronEfficiency,
                SaleFlag = 0
            };

            packet.Sender.Send(ack.CreatePacket());
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
            var chatMsgPacket = new ChatMessagePacket(packet);

            var sender = packet.Sender.User.ActiveCharacter.Name;

            Log.Debug($"({chatMsgPacket.MessageType}) <{sender}> {chatMsgPacket.Message}");

            var ack = new ChatMessageAnswer
            {
                MessageType = chatMsgPacket.MessageType,
                SenderCharacterName = sender,
                Message = chatMsgPacket.Message
            };

            var ackPkt = ack.CreatePacket();

            switch (chatMsgPacket.MessageType)
            {
                case "room":
                    GameServer.Instance.Server.Broadcast(ackPkt); // TODO: broadcast only to users in same area
                    break;

                case "channel":
                    GameServer.Instance.Server.Broadcast(ackPkt);
                    break;

                case "party":
                    GameServer.Instance.Server.Broadcast(ackPkt); // TODO: broadcast only to users in same party
                    break;

                case "team":
                    GameServer.Instance.Server.Broadcast(ackPkt); // TODO: broadcast only to users in same crew
                    break;

                default:
                    Log.Error("Undefined chat message type.");
                    break;
            }
        }

        [Packet(Packets.CmdMyTeamInfo)]
        public static void MyTeamInfo(Packet packet)
        {
            var myTeamInfoPacket = new MyTeamInfoPacket(packet);
            
            var ack = new MyTeamInfoAnswer
            {
                Action = myTeamInfoPacket.Action,
                CharacterId = packet.Sender.User.ActiveCharacterId,
                Rank = 0,
                Team = packet.Sender.User.ActiveTeam,
                Age = 0
            };

            packet.Sender.Send(ack.CreatePacket());
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
            var gameCharInfoPacket = new GameCharInfoPacket(packet);
            //gameCharInfoPacket.CharacterName <-- Unused for now!

            var ack = new GameCharInfoAnswer
            {
                Character = packet.Sender.User.ActiveCharacter,
                Vehicle = packet.Sender.User.ActiveCar,
                StatisticInfo = new StatInfo(),
                Team = packet.Sender.User.ActiveTeam,
                Serial = 0,
                LocType = 'A',
                ChId = 'A',
                LocId = 1
            };
            packet.Sender.Send(ack.CreatePacket());
        }

        [Packet(Packets.CmdChaseRequest)]
        public static void ChaseRequest(Packet packet)
        {
            /*
            [Debug] - ChaseRequest: 000000: 01 9A 45 91 45 66 4A AA 45 A6 52 E7 40 00 00 00  · · E · E f J · E · R · @ · · ·
            000016: 00  ·

            [Info] - Received ChaseRequest (id 189, 0xBD) on 11021.
            */

            var chaseRequestPacket = new ChaseRequestPacket(packet);

            var ack = new ChaseRequestAnswer
            {
                StartPosX = chaseRequestPacket.PosX,
                StartPosY = chaseRequestPacket.PosY,
                StartPosZ = chaseRequestPacket.PosZ,
                StartRot = chaseRequestPacket.Rot,
                EndPosX = chaseRequestPacket.PosX,
                EndPosY = chaseRequestPacket.PosY,
                EndPosZ = chaseRequestPacket.PosZ,
                EndRot = chaseRequestPacket.Rot,
                CourseId = 0,
                Type = 2 - (chaseRequestPacket.BNow ? 1 : 0),
                PosName = "test",
                FirstHuvLevel = 1,
                FirstHuvId = 10001
            };

            //Wrong Packet Size. CMD(186) CmdLen: : 252, AnalysisSize: 250
            /*var ack = new Packet(Packets.ChasePropose);
            ack.Writer.Write((ushort) 0);
            ack.Writer.Write(chaseRequestPacket.PosX); // Start X
            ack.Writer.Write(chaseRequestPacket.PosY); // Start Y
            ack.Writer.Write(chaseRequestPacket.PosZ); // Start Z
            ack.Writer.Write(chaseRequestPacket.Rot); // Start W

            ack.Writer.Write(chaseRequestPacket.PosX); // End X
            ack.Writer.Write(chaseRequestPacket.PosY); // End Y
            ack.Writer.Write(chaseRequestPacket.PosZ); // End Z

            ack.Writer.Write(0); // CourseId
            ack.Writer.Write(2 - (chaseRequestPacket.BNow ? 1 : 0)); // Type?
            ack.Writer.WriteUnicodeStatic("test", 100);

            ack.Writer.Write(1); // HUV first level
            ack.Writer.Write(10001); // HUV first Id
            ack.Writer.Write(new byte[2]); // Not sure.
            */
            packet.Sender.Send(ack.CreatePacket());
        }

        [Packet(Packets.CmdInstantStart)]
        public static void InstantStart(Packet packet)
        {
            var instantStartPacket = new InstantStartPacket(packet);

            var ack = new InstantStartAnswer {TableIndex = instantStartPacket.TableIndex};

            packet.Sender.Send(ack.CreatePacket());
        }

        [Packet(Packets.CmdInstantGoalPlace)]
        public static void InstantGoalPlace(Packet packet)
        {
            var instantGoalPlacePacket = new InstantGoalPlacePacket(packet);

            var ack = new InstantGoalPlaceAnswer
            {
                TableIndex = instantGoalPlacePacket.TableIndex,
                PlaceIndex = instantGoalPlacePacket.PlaceIndex,
                EXP = 0
            };

            packet.Sender.Send(ack.CreatePacket());
        }

        [Packet(Packets.CmdInstantGiveUp)]
        public static void InstantGiveUp(Packet packet)
        {
            var instantGiveUpPacket = new InstantGiveUpPacket(packet);

            var ack = new InstantGiveUpAnswer {TableIndex = instantGiveUpPacket.TableIndex};

            packet.Sender.Send(ack.CreatePacket());
        }

        //000000: 01 00 00 00  · · · ·
        [Packet(Packets.CmdQuestStart)]
        public static void QuestStart(Packet packet)
        {
            var questStartPacket = new QuestStartPacket(packet);

            QuestModel.Add(GameServer.Instance.Database.Connection, new Quest
            {
                CharacterId = packet.Sender.User.ActiveCharacterId,
                CharacterName = packet.Sender.User.ActiveCharacter.Name,
                FailNum = 0,
                PlaceIdx = 0,
                QuestId = questStartPacket.TableIndex,
                ServerId = 0,
                State = 0
            });

            var ack = new QuestStartAnswer
            {
                TableIndex = questStartPacket.TableIndex,
                FailNum = 0
            };
            packet.Sender.Send(ack.CreatePacket());
        }

        [Packet(Packets.CmdQuestGiveUp)]
        public static void QuestGiveup(Packet packet)
        {
            var questGiveUpPacket = new QuestGiveUpPacket(packet);

            QuestModel.Update(GameServer.Instance.Database.Connection, 0, packet.Sender.User.ActiveCharacterId,
                questGiveUpPacket.TableIndex, 4);

            var ack = new QuestGiveUpAnswer {TableIndex = questGiveUpPacket.TableIndex};

            packet.Sender.Send(ack.CreatePacket());
        }

        //000000: 01 00 00 00  · · · ·
        [Packet(Packets.CmdQuestGoalPlace)]
        public static void QuestGoalPlace(Packet packet)
        {
            #if !DEBUG
            throw new NotImplementedException();
            #endif
        }

        //000000: 01 00 00 00 14 43 8B 42 4E 9E D0 FE  · · · · · C · B N · · ·
        [Packet(Packets.CmdQuestComplete)]
        public static void QuestComplete(Packet packet)
        {
            var questCompletePacket = new QuestCompletePacket(packet);
            
            QuestModel.Update(GameServer.Instance.Database.Connection, 0, packet.Sender.User.ActiveCharacterId,
                questCompletePacket.TableIndex, 1);

            var ack = new QuestCompleteAnswer {TableIndex = questCompletePacket.TableIndex};
            packet.Sender.Send(ack.CreatePacket());
        }

        [Packet(Packets.CmdQuestReward)]
        public static void QuestReward(Packet packet)
        {
            var questRewardPacket = new QuestRewardPacket(packet);

            var quest = QuestModel.RetrieveOne(GameServer.Instance.Database.Connection, 0,
                packet.Sender.User.ActiveCharacterId,
                questRewardPacket.TableIndex);
            QuestModel.Update(GameServer.Instance.Database.Connection, 0, packet.Sender.User.ActiveCharacterId,
                questRewardPacket.TableIndex, 2);
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

            if (!GameServer.QuestTable.ContainsKey(questRewardPacket.TableIndex))
            {
                packet.Sender.Error("Quest reward not found.");
                return;
            }
            var questReward = GameServer.QuestTable[questRewardPacket.TableIndex];
            packet.Sender.User.ActiveCharacter.CurExp += questReward.RewardExp;
            // TODO: Check if user has leveled up.

            // TODO: Load quest reward information here, and send it.

            var ack = new QuestRewardAnswer
            {
                TableIndex = questReward.QuestIdN,
                GetExp = (uint) questReward.RewardExp,
                GetMoney = (uint) questReward.RewardMoney,
                CurrentExp = (ulong) packet.Sender.User.ActiveCharacter.CurExp,
                NextExp = (ulong) packet.Sender.User.ActiveCharacter.NextExp,
                BaseExp = (ulong) packet.Sender.User.ActiveCharacter.BaseExp,
                Level = packet.Sender.User.ActiveCharacter.Level,
                ItemNum = 0,
                RewardItem1 = 0,
                RewardItem2 = 0,
                RewardItem3 = 0
            };
            packet.Sender.Send(ack.CreatePacket());

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
            var cityLeaveCheckPacket = new CityLeaveCheckPacket(packet);

            Console.WriteLine(cityLeaveCheckPacket.Post2);

            var ack = new CityLeaveCheckAnswer
            {
                Result = 1,
                CityId = cityLeaveCheckPacket.CityId,
                Post1 = cityLeaveCheckPacket.Post1,
                Post2 = cityLeaveCheckPacket.Post2
            };
            packet.Sender.Send(ack.CreatePacket());

            /* Pulled from Rice:
            var ack = new RicePacket(3201);
            ack.Writer.Write(1); // if ( *(_DWORD *)(pktBuf + 2) == 1 )
            ack.Writer.Write(packet.Reader.ReadBytes(514)); // apparently these fuckers want their own 514 bytes back
            packet.Sender.Send(ack);
            */

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
            var buyCarPacket = new BuyCarPacket(packet);

            var ack = new BuyCarAnswer
            {
                Id = 1,
                CarType = buyCarPacket.CarType,
                BaseColor = 0,
                Grade = 0,
                SlotType = 0,
                AuctionCount = 0,
                Fuel = 100.0f,
                Kilometer = 0.0f,
                Color = buyCarPacket.Color,
                FuelCapacity = 0.0f,
                FuelEfficiency = 0.0f,
                AuctionOn = false,
                Unknown1 = 0,
                Unknown2 = 0,
                Price = 10000
            };
            // Per hour!?
            packet.Sender.Send(ack.CreatePacket());
            
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
            var ack = new GetMyHancoinAnswer
            {
                Hancoins = 10000,
                Mileage = 100
            };

            packet.Sender.Send(ack.CreatePacket());
        }

        [Packet(Packets.CmdCheckStat)]
        public static void CheckStat(Packet packet)
        {
            var ack = new CheckStatAnswer()
            {
                CarSpeed = 0,
                CarDurability = 0,
                CarAcceleration = 0,
                CarBoost = 0,
        
                PartSpeed = 0,
                PartDurability = 0,
                PartAcceleration = 0,
                PartBoost = 0,
        
                UserSpeed = 0,
                UserDurability = 0,
                UserAcceleration = 0,
                UserBoost = 0,
        
                CharSpeed = 0,
                CharDurability = 0,
                CharAcceleration = 0,
                CharBoost = 0,

                ItemUseSpeed = 0,
                ItemUseCrash = 0,
                ItemUseAcceleration = 0,
                ItemUseBoost = 0,
        
                VehicleSpeed = 0,
                VehicleDurability = 0,
                VehicleAcceleration = 0,
                VehicleBoost = 0,
            };
            
            packet.Sender.Send(ack.CreatePacket());
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
            var buyItemPacket = new BuyItemPacket(packet);

            var ack = new BuyItemAnswer()
            {
                ItemId = buyItemPacket.ItemId,
                Quantity = buyItemPacket.Quantity,
                Gold = 200,
            };
            packet.Sender.Send(ack.CreatePacket());

            var modListAnswer = new ItemModListAnswer()
            {
                Items = new []
                {
                    new XiStrMyItemMod()
                    {
                        MyItem = new Item()
                        {
                            CarID = 4,
                            Itm = new ItemData()
                            {
                                Slot = 1,
                                State = 1,
                                StateVar = 1,
                            },
                            iunit = new IUnit()
                            {
                                StackNum = buyItemPacket.Quantity,
                                Random = 0,
                                AssistA = 0,
                                AssistB = 0,
                                Box = 0,
                                Belonging = 0,
                                Upgrade = 0,
                                UpgradePoint = 0,
                                ExpireTick = 0,
                            },
                            TableIdx = (uint)buyItemPacket.ItemId,
                            InvenIdx = 0,
                        },
                        State = 0,
                    },
                }
            };
            packet.Sender.Send(modListAnswer.CreatePacket());
            
            #if !DEBUG
            throw new NotImplementedException();
            #endif

            //BS_PktItemDataList

            //PacketSend::Send_ItemModList((BS_PacketDispatch *)&pGameDispatch->vfptr);
        }

        [Packet(Packets.CmdBuyVisualItemThread)]
        public static void BuyVisualItemThread(Packet packet)
        {
            var buyVisualItemPacket = new BuyVisualItemThreadPacket(packet);

            var ack = new BuyVisualItemThreadAnswer()
            {
                Type = 0,
                TableIndex = buyVisualItemPacket.TableIndex,
                CarId = buyVisualItemPacket.CarId,
                InventoryId = 0,
                Period = 5,
                Mito = 0,
                Hancoin = 0,
                BonusMito = 0,
                Mileage = 0
            };
            packet.Sender.Send(ack.CreatePacket());

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
                    TeamMarkId = packet.Sender.User.ActiveCharacter.TeamMarkId,
                    TeamName = packet.Sender.User.ActiveCharacter.TeamName,
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
                        Reserve = new short[]{0,0,0,0,0,0},
                        PlateString = "HELLO",
                    },
                    UseTime = 100.0f,
                }
            };
            packet.Sender.Send(ack2.CreatePacket());

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
        /* Mito gotcha play: packet number 3500*/

        /*BuyHistoryList: int32 pageNumber, int32 unknown, int32 tab (1=purchase history, 2=sent gift, 3=received gift)*/
    }
}