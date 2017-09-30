using System;
using System.Numerics;
using Microsoft.SqlServer.Server;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Util;

namespace GameServer.Network.Handlers
{
    internal class Joining
    {
        [Packet(Packets.CmdCheckInGame)]
        public static void CheckInGame(Packet packet)
        {
            var checkInGamePacket = new CheckInGamePacket(packet);
            // TODO: Check packet here!
            var ack = new CheckInGameAnswer()
            {
                Result = 1
            };
            packet.Sender.Send(ack.CreatePacket());
        }

        [Packet(Packets.CmdJoinChannel)]
        public static void JoinChannel(Packet packet)
        {
            var serial = GameServer.Instance.Server.LastSerial++;
            GameServer.Instance.Server.ActiveSerials.Add(serial, packet.Sender.User);
            packet.Sender.User.ActiveCharacter.VehicleSerial = serial;
            if (!AccountModel.UpdateVehicleSerial(GameServer.Instance.Database.Connection, packet.Sender.User.Id, serial))
            {
                packet.Sender.KillConnection("Failed to update serial.");
                return;
            }
            
            packet.Sender.Send(new JoinChannelAnswer()
            {
                ChannelName = "speeding",
                CharacterName = packet.Sender.User.ActiveCharacter.Name,
                Serial = (short)serial,
                SessionAge = 0,
            }.CreatePacket());

            packet.Sender.Send(new WeatherAnswer()
            {
                CurrentWeather = WeatherAnswer.Weather.Rain
            }.CreatePacket());
            
            // As per legal requirements, this shall not be removed!
            packet.Sender.SendChatMessage($"Server powered by DCNC (v{Shared.Util.Version.GetVersion()}) - GigaToni");
            // As per legal requirements, this shall not be removed!
        }

        [Packet(Packets.CmdJoinArea)]
        public static void JoinArea(Packet packet)
        {
            var joinAreaPacket = new JoinAreaPacket(packet);
            packet.Sender.Send(new JoinAreaAnswer()
            {
                AreaId = joinAreaPacket.AreaId,
                Result = 1,
            }.CreatePacket());
        }

        /*[Packet(Packets.CmdGateList)]
        public static void GateList(Packet packet)
        {
            
        }*/
        
        [Packet(Packets.CmdFirstPosition)] // TODO: Actual position and not just dummies
        public static void FirstPosition(Packet packet)
        {
            var character = packet.Sender.User.ActiveCharacter;
            var ack = new MyCityPositionAnswer();
            ack.PositionState = character.PosState;
            if (packet.Sender.User.ActiveCharacter.PosState != 1)
            {
                if (packet.Sender.User.ActiveCharacter.PosState == 2)
                {
                    ack.City = character.City;
                    ack.LastChannel = RandomProvider.Get().Next(0, 10);
                    /*
                    if ( *(_BYTE *)&BS_Global::ContentsFlag.Main.0 >> 8 )
                        lpAckPkt->m_ChannelId = rand() % 2;
                    */
                    ack.Position = character.Position; // AreaPos?
                }
                else
                {
                    ack.City = 0;
                    ack.LastChannel = packet.Sender.User.ActiveCharacter.LastChannel;
                    ack.Position = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
                }
            }
            else
            {
                character.City = 1;
                character.PosState = 1;
                
                ack.City = 1;
                ack.Position.X = -2157.2f + 4.0f * RandomProvider.Get().Next(0, 10);
                ack.Position.Y = -205.05f + 4.0f * RandomProvider.Get().Next(0, 10);
                ack.Position.Z = 85.720001f + 4.0f * RandomProvider.Get().Next(0, 10);
                ack.Position.W = 90.967003f + 4.0f * RandomProvider.Get().Next(0, 10);
                ack.LastChannel = -1;
            }
                
            // Check dormant event?
            
            packet.Sender.Send(ack.CreatePacket());

            if (character.PosState == 1 && character.Level == 1)
            {
                // Recommend friend
            }
            
            // Team notify
            
            /*packet.Sender.Send(new FirstPositionAnswer()
            {
                City = packet.Sender.User.ActiveCharacter.City,
                LastChannel = packet.Sender.User.ActiveCharacter.LastChannel,
                Position = packet.Sender.User.ActiveCharacter.Position,
                PositionState = packet.Sender.User.ActiveCharacter.PosState
            }.CreatePacket());*/
        }

        [Packet(Packets.CmdAreaList)]
        public static void AreaList(Packet packet)
        {
            // client calls 2 functions (not using any packet data), returns  137 * (*(_DWORD *)(pktBuf + 2) - 1) + 143;
            var ack = new Packet(Packets.AreaListAck);
            ack.Writer.Write(1);
            ack.Writer.Write(new byte[137]);
            packet.Sender.Send(ack);
        }

        /*
        41 00 64 00 6D 00 69 00 6E 00 69 00 73 00 74 00  A · d · m · i · n · i · s · t ·
        72 00 61 00 74 00 6F 00 72 00 00 00 00 00 00 00  r · a · t · o · r · · · · · · ·
        00 00 00 00 00 00 00 00 00 00 01 00 00 00  · · · · · · · · · · · · · · 
        */
        [Packet(Packets.CmdLoadCharThread)]
        public static void LoadCharThread(Packet packet)
        {
            /*
            struct __cppobj __unaligned __declspec(align(2)) BS_PktETCInfo : BS_PktBody
            {
              int m_IsPcBang;
              int m_NumPCBangCoupon;
              int m_reserve[32];
            };
            */

            var loadCharThreadPacket = new LoadCharThreadPacket(packet);

            var character = CharacterModel.Retrieve(GameServer.Instance.Database.Connection, loadCharThreadPacket.CharacterName);
            if (character == null)
            {
                packet.Sender.KillConnection("Invalid character selected.");
                return;
            }
            
            // Even though we set it already it does seem to not work for some reason.
            //var team = TeamModel.Retrieve(GameServer.Instance.Database.Connection, character.TeamId); 
            /*
            character.TeamId = team.TeamId;
            character.TeamName = team.TeamName;
            character.TeamMarkId = team.TeamMarkId;
            character.TeamCloseDate = (int) team.CloseDate;
            character.TeamRank = 1;*/ // <-- This all get set by CharacterModel already.

            var user = AccountModel.Retrieve(GameServer.Instance.Database.Connection, character.Uid);
            AccountModel.SetActiveCharacter(GameServer.Instance.Database.Connection, user, character.Id);

            packet.Sender.User = user;
            packet.Sender.User.ActiveCharacterId = character.Id;
            packet.Sender.User.ActiveCharacter = character;
            packet.Sender.User.ActiveCharacter.ActiveCar.CarID = character.ActiveVehicleId;
            //packet.Sender.User.ActiveCharacter.Team = team;
            packet.Sender.User.Characters = AccountModel.RetrieveCharacters(GameServer.Instance.Database.Connection, user.Id);
            //packet.Sender.User.Characters = CharacterModel.RetrieveUser(GameServer.Instance.Database.Connection, user.Id);

            var vehicles = VehicleModel.Retrieve(GameServer.Instance.Database.Connection, character.Id);

            packet.Sender.User.ActiveCharacter.ActiveCar = vehicles.Find(vehicle => vehicle.CarID == character.ActiveVehicleId);
            var ack = new LoadCharThreadAnswer()
            {
                ServerId = 0,
                ServerStartTime = 0,
                Character = character,
                Vehicles = vehicles.ToArray(),
                CurrentCarId = (int)character.ActiveVehicleId,
            };
            packet.Sender.Send(ack.CreatePacket());
            
            packet.Sender.Send(new StatUpdateAnswer()
            {
                StatisticInfo = new XiStrStatInfo()
                {
                    BasedAccel = 0,
                    BasedBoost = 0,
                    BasedCrash = 0,
                    BasedSpeed = 0,
                    CharAccel = 0,
                    CharBoost = 0,
                    CharCrash = 0,
                    CharSpeed = 0,
                    EquipAccel = 0,
                    EquipBoost = 0,
                    EquipCrash = 0,
                    EquipSpeed = 0,
                    ItemUseAccel = 0,
                    ItemUseBoost = 0,
                    ItemUseCrash = 0,
                    ItemUseSpeed = 0,
                    TotalAccel = 0,
                    TotalBoost = 0,
                    TotalCrash = 0,
                    TotalSpeed = 0,
                },
                EnchantBonus = new XiStrEnchantBonus()
                {
                    Speed = 0,
                    Crash = 0,
                    Accel = 0,
                    Boost = 0,
                    AddSpeed = 0,
                    Drop = 0.0f,
                    Exp = 0.0f,
                    MitronCapacity = 0.0f,
                    MitronEfficiency = 0.0f
                }
            }.CreatePacket());

            /*var character = Rice.Game.Character.Retrieve(characterName); // TODO: verify this
            var user = Rice.Game.User.Retrieve(character.UID);

            packet.Sender.Player = new Rice.Game.Player(user);
            packet.Sender.Player.Characters = Rice.Game.Character.Retrieve(user.UID);
            packet.Sender.Player.ActiveCharacter = character;

            var ack = new RicePacket(124);

            var cars = Rice.Game.Vehicle.Retrieve(character.CID);

            var ackStruct = new Structures.LoadCharAck
            {
                CharInfo = character.GetInfo(),
                nCarSize = (uint)cars.Count,
                CarInfo = cars,
            };
            ack.Writer.Write(ackStruct);

            packet.Sender.Send(ack);
            Log.WriteLine("Sent LoadCharAck");*/

            /* Pulled from Rice:
            var stat = new RicePacket(760); // StatUpdate
            for (int i = 0; i < 16; ++i)
                stat.Writer.Write(0);
            stat.Writer.Write(1000);
            stat.Writer.Write(9001);
            stat.Writer.Write(9002);
            stat.Writer.Write(9003);
            stat.Writer.Write(new byte[76]);
            packet.Sender.Send(stat);
            */
        }

        /*
        00 00 00 00  · · · · 
        * uint GivePostIdx
        */
        [Packet(Packets.CmdMyQuestList)]
        public static void MyQuestList(Packet packet) // TODO: Send actual data
        {
            var quests = QuestModel.Retrieve(GameServer.Instance.Database.Connection, 0,
                packet.Sender.User.ActiveCharacterId);

            var ack = new MyQuestListAnswer
            {
                Quests = quests.ToArray()
            };
            packet.Sender.Send(ack.CreatePacket());
        }

        [Packet(Packets.CmdItemList)]
        public static void ItemList(Packet packet)
        {
            ItemModel.RetrieveAll(GameServer.Instance.Database.Connection,
                ref packet.Sender.User.ActiveCharacter);
            
            var items = ItemModel.RetrieveAll(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId);
            var ack = new ItemListAnswer
            {
                InventoryItems = items.ToArray()
            };

            packet.Sender.Send(ack.CreatePacket());
        }

        [Packet(Packets.CmdVisualItemList)]
        public static void VisualItemList(Packet packet)
        {
            var ack = new Packet(Packets.VisualItemListAck);
            ack.Writer.Write(262144); // ListUpdate (262144 = First packet from list queue, 262145 = sequential)
            //packet.Sender.User.ActiveCharacter.InventoryVisualItems.Count
            ack.Writer.Write(0); // ItemNum
            ack.Writer.Write(new byte[120]); // Null VisualItem (120 bytes per XiStrMyVSItem)
            packet.Sender.Send(ack);
        }

        [Packet(Packets.CmdPlayerInfoReq)]
        public static void PlayerInfoReq(Packet packet)
        {
            var reqCnt = packet.Reader.ReadUInt32();
            var serial = packet.Reader.ReadUInt16(); // No known scenarios where the requested info count is > 1
            
            foreach (var client in GameServer.Instance.Server.GetClients())
            {
                if (client.User.ActiveCharacter == null || client.User.VehicleSerial != serial) continue;
                
                var character = client.User.ActiveCharacter;
                var playerInfo = new XiPlayerInfo()
                {
                    CharacterName = character.Name,
                    Serial = (ushort)client.User.VehicleSerial, // TODO: Verify if the serial is the same as carid
                    Age = 0
                };
                var res = new Packet(Packets.PlayerInfoOldAck);
                res.Writer.Write(1); // Result
                res.Writer.Write(playerInfo);
                packet.Sender.Send(res);
                break;
            }
        }

        /* Pulled from Rice:
		[RicePacket(801, RiceServer.ServerType.Game)]
        public static void PlayerInfoReq(RicePacket packet)
        {
            var reqCnt = packet.Reader.ReadUInt32();
            var serial = packet.Reader.ReadUInt16(); // No known scenarios where the requested info count is > 1
            // Followed by the session age of the player we are requesting info for. nplutowhy.avi
 
            foreach(var p in RiceServer.GetPlayers())
            {
                if(p.ActiveCharacter != null && p.ActiveCharacter.CarSerial == serial)
                {
                    var character = p.ActiveCharacter;
                    var playerInfo = new Structures.PlayerInfo()
                    {
                        Name = character.Name,
                        Serial = character.CarSerial,
                        Age = 0
                    };
                    var res = new RicePacket(802);
                    res.Writer.Write(1);
                    res.Writer.Write(playerInfo);
                    packet.Sender.Send(res);
                    break;
                }
            }
        }
		*/
    }
}