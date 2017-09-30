using System;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Microsoft.SqlServer.Server;
using Shared;
using Shared.Models;
using Shared.Network;
using Shared.Network.AuthServer;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Util;
using Shared.Util.Commands;

// ReSharper disable UnusedMember.Global

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
            int channelId = packet.Reader.ReadInt32();
            //Console.WriteLine(packet.Reader.ReadUInt32());
            // SHORT CHANNELID
            // -> Gate ID!

            var character = packet.Sender.User.ActiveCharacter;
            packet.Sender.Send(new MyCityPositionAnswer()
            {
                City = character.City,
                LastChannel = channelId,
                Position = character.Position,
                PositionState = character.PosState
            }.CreatePacket());
        }

        /*
        
        */
        [Packet(Packets.CmdSaveCarPos)]
        public static void SaveCarPos(Packet packet)
        {
            var saveCar = new SaveCarPosPacket(packet);

            packet.Sender.User.ActiveCharacter.LastChannel = saveCar.ChannelId;
            packet.Sender.User.ActiveCharacter.City = saveCar.CityId;
            packet.Sender.User.ActiveCharacter.Position = saveCar.Position;
            packet.Sender.User.ActiveCharacter.PosState = saveCar.PosState;

            CharacterModel.Update(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacter);
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


        /// <summary>
        /// Handles the fuel charge packet
        /// TODO: Move Sale price to a server settings
        /// </summary>
        /// <param name="packet"></param>
        [Packet(Packets.CmdFuelChargeReq)]
        public static void FuelChargeReq(Packet packet)
        {
            var fuelChargeReqPacket = new FuelChargeReqPacket(packet);

            // Update money first
            packet.Sender.User.ActiveCharacter.MitoMoney =
                packet.Sender.User.ActiveCharacter.MitoMoney - fuelChargeReqPacket.Pay;
            CharacterModel.Update(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacter);

            // Update vehicle fuel
            packet.Sender.User.ActiveCharacter.ActiveCar.Mitron += fuelChargeReqPacket.Fuel;
            VehicleModel.Update(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacter.ActiveCar);

            var ack = new FuelChargeReqAnswer
            {
                CarId = fuelChargeReqPacket.CarId,
                Pay = fuelChargeReqPacket.Pay,
                Fuel = packet.Sender.User.ActiveCharacter.ActiveCar.Mitron,
                Gold = packet.Sender.User.ActiveCharacter.MitoMoney,
                DeltaFuel = fuelChargeReqPacket.Fuel,
                SaleUnitPrice = 20.0f,
                DiscountedSaleUnitPrice = 15.0f,
                FuelCapacity = packet.Sender.User.ActiveCharacter.ActiveCar.MitronCapacity,
                FuelEfficiency = packet.Sender.User.ActiveCharacter.ActiveCar.MitronEfficiency,
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

        [Packet(Packets.CmdGameStream)]
        public static void GameStream(Packet packet)
        {
            var unknown1 = packet.Reader.ReadInt16();

            var message = packet.Reader.ReadUnicode();

            var args = ConsoleUtil.ParseLine(message);
            if (args.Count <= 0) return;
            var cmd = args[0];
            args.RemoveAt(0); // Remove command itself

            var command = GameServer.ChatCommands.GetCommand(cmd);

            if (command == null) return;

            if (packet.Sender.User.Permission < command.RequiredPermission)
            {
                Log.Warning("User tried to use admin command: " + command.Name);
                return;
            }

            var res = command.Func(GameServer.Instance.Server, packet.Sender, cmd, args);
            if (res == CommandResult.InvalidArgument)
            {
                packet.Sender.SendChatMessage("Syntax: " + command.Usage);
            }
        }

        

        [Packet(Packets.CmdMyTeamInfo)]
        public static void MyTeamInfo(Packet packet)
        {
            var myTeamInfoPacket = new MyTeamInfoPacket(packet);
            
            var user = packet.Sender.User;
            
            var ack = new MyTeamInfoAnswer
            {
                Action = myTeamInfoPacket.Action,
                CharacterId = user.ActiveCharacterId,
                Rank = 0,
                Team = user.ActiveCharacter.Team,
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

            // TODO: Combine this into 1 MySQL Query.
            var character = CharacterModel.Retrieve(GameServer.Instance.Database.Connection,
                //"GigaToni2");
                gameCharInfoPacket.CharacterName);
            /*character.ActiveCar =
                VehicleModel.Retrieve(GameServer.Instance.Database.Connection, (uint) character.CurrentCarId);*/
            //var activeTeam = TeamModel.Retrieve(GameServer.Instance.Database.Connection, character.TeamId);
            
            var ack = new GameCharInfoAnswer
            {
                Character = character,
                Vehicle = character.ActiveCar,
                StatisticInfo = new XiStrStatInfo(),
                Team = character.Team,
                Serial = character.VehicleSerial,
                //LocType = 'A',
                ChId = (char)character.LastChannel, //'A',
                //LocId = 1
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
                StartPos = new Vector4(chaseRequestPacket.PosX, chaseRequestPacket.PosY, chaseRequestPacket.PosZ, chaseRequestPacket.Rot),
                EndPos = new Vector4(chaseRequestPacket.PosX, chaseRequestPacket.PosY, chaseRequestPacket.PosZ, chaseRequestPacket.Rot),
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
            Log.Unimplemented("Not implemented");
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
                packet.Sender.SendError("Quest was not started!");
                return;
            }
            if (quest.State != 1)
            {
                packet.Sender.SendError("Quest not finished!");
                return;
            }

            var questReward =
                ServerMain.Quests.Find(quest1 => quest1.TableIndex == questRewardPacket.TableIndex);
            if (questReward == null)
            {
                packet.Sender.SendError("Quest reward not found.");
                return;
            }
            var itemReward = questReward.GetRewards();
            var character = packet.Sender.User.ActiveCharacter;

            bool levelUp;
            const bool useBonus = false; // TODO: Determine boni
            const bool useBonus500Mita = false;
            character.CalculateExp(questReward.Experience, out levelUp, useBonus,
                useBonus500Mita);

            CharacterModel.Update(GameServer.Instance.Database.Connection, character);
            
            var item01 = 0;
            var item02 = 0;
            var item03 = 0;
            if (itemReward.Length > 0)
            {
                if (itemReward.Length >= 1)
                {
                    item01 = ServerMain.Items.FindIndex(item => item.Id == itemReward[0]);
                    if (item01 == -1 || character.GiveItem(GameServer.Instance.Database.Connection, item01, 1) ==
                        null)
                        item01 = 0;
                }
                if (itemReward.Length >= 2)
                {
                    item02 = ServerMain.Items.FindIndex(item => item.Id == itemReward[1]);
                    if (item01 == -1 || character.GiveItem(GameServer.Instance.Database.Connection, item02, 1) ==
                        null)
                        item02 = 0;
                }
                if (itemReward.Length == 3)
                {
                    item03 = ServerMain.Items.FindIndex(item => item.Id == itemReward[2]);
                    if (item01 == -1 || character.GiveItem(GameServer.Instance.Database.Connection, item03, 1) ==
                        null)
                        item03 = 0;
                }
            }
            
            var ack = new QuestRewardAnswer
            {
                TableIndex = (uint) questReward.TableIndex,
                GetExp = (uint) questReward.Experience,
                GetMoney = (uint) questReward.Mito,
                CurrentExp = (ulong) character.ExperienceInfo.CurExp,
                NextExp = (ulong) character.ExperienceInfo.NextExp,
                BaseExp = (ulong) character.ExperienceInfo.BaseExp,
                Level = character.Level,
                ItemNum = (ushort) itemReward.Length,
                RewardItem1 = (uint)item01,
                RewardItem2 = (uint)item02,
                RewardItem3 = (uint)item03
            };
            packet.Sender.Send(ack.CreatePacket());
            
            if(item01 != 0 || item02 != 0 || item03 != 0)
                character.FlushItemModBuffer(packet.Sender);
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

            /*
             * Used bytes are:
             * 2 - 6 -> Result? // int?
             * 6 - 10 -> CityId? // int?
             * 10 - ?? -> Some kind of string, maybe Gate? // 255?
             * 265 - ?? -> Some kind of string, maybe Gate? // 255?
             */
        }

        [Packet(Packets.CmdDriveInfoUpdate)]
        public static void DriveInfoUpdate(Packet packet)
        {
            var driveInfo = new DriveInfoPacket(packet);

            var fDeltaFuel = packet.Sender.User.ActiveCharacter.ActiveCar.Mitron - driveInfo.TotalFuel;
            if (fDeltaFuel > 0.0f)
                packet.Sender.User.ActiveCharacter.ActiveCar.Mitron -= fDeltaFuel;
            var fDelta = driveInfo.TotalDistance - packet.Sender.User.ActiveCharacter.ActiveCar.Kmh;
            if (fDelta > 0.0f)
            {
                packet.Sender.User.ActiveCharacter.ActiveCar.Kmh += fDelta;
                packet.Sender.User.ActiveCharacter.TotalDistance += fDelta;
            }

            if (packet.Sender.User.ActiveCharacter.ActiveCar.Mitron <= 0.0f)
                packet.Sender.User.ActiveCharacter.ActiveCar.Mitron = 0.0f;

            // Save car to db.
            VehicleModel.Update(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacter.ActiveCar);


            /*
            fDeltaFuel = pCarInfo->m_CarInfo.CarUnit.Mitron - *(float *)(lpBuffer + 14);
            if ( fDeltaFuel > 0.0 )
              pCarInfo->m_CarInfo.CarUnit.Mitron = pCarInfo->m_CarInfo.CarUnit.Mitron - fDeltaFuel;
            fDelta = *(float *)(lpBuffer + 10) - pCarInfo->m_CarInfo.CarUnit.Kmh;
            if ( fDelta > 0.0 )
            {
              pCarInfo->m_CarInfo.CarUnit.Kmh = pCarInfo->m_CarInfo.CarUnit.Kmh + fDelta;
              pCharInfo->m_CharInfo.TotalDistance = pCharInfo->m_CharInfo.TotalDistance + fDelta;
              pCarInfo->m_fFuelConsume = pCarInfo->m_fFuelConsume + fDelta;
            }
            pGame->m_lastUpdateTime = GetSystemTick();
            if ( pCarInfo->m_CarInfo.CarUnit.Mitron < 0.0 )
              pCarInfo->m_CarInfo.CarUnit.Mitron = *(float *)&FLOAT_0_0;
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

        public static class Tutorial
        {
            [Packet(Packets.CmdTutorialClear)]
            public static void TutorialClear(Packet packet)
            {
                var type = packet.Reader.ReadUInt32();
                var ack = new Packet(Packets.TutorialClearAck);
                ack.Writer.Write(type);

                packet.Sender.Send(ack);
            }
        }

        [Packet(Packets.CmdCheckStat)]
        public static void CheckStat(Packet packet)
        {
            var ack = new CheckStatAnswer()
            {
                /*CarSpeed = 0,
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
                VehicleBoost = 0,*/
				// TODO: Nothing.
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

        [Packet(Packets.CmdSellItem)]
        public static void SellItem(Packet packet)
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
        
        /* Mito gotcha play: packet number 3500*/

        /*BuyHistoryList: int32 pageNumber, int32 unknown, int32 tab (1=purchase history, 2=sent gift, 3=received gift)*/


        [Packet(Packets.CmdSelectCar)]
        public static void SelectCar(Packet packet)
        {
            var character = packet.Sender.User.ActiveCharacter;
            
            var vehId = packet.Reader.ReadUInt32();
            var vehicle = character.GarageVehicles.FirstOrDefault(veh => veh.CarID == vehId);
            if (vehicle == null)
            {
                Log.Error("User tried to enter car he doesn't own!");
                packet.Sender.KillConnection("Hack attempt blocked!");
                return;
            }

            packet.Sender.User.ActiveCharacter.ActiveVehicleId = vehId;
            packet.Sender.User.ActiveCharacter.ActiveCar = vehicle;
            CharacterModel.Update(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacter);

            packet.Sender.Send(new SelectCarAnswer()
            {
                Vehicle = vehicle,
            }.CreatePacket());
        }
    }
}