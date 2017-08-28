using System;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;

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
            packet.Sender.Send(new JoinChannelAnswer()
            {
                ChannelName = "speeding",
                CharacterName = packet.Sender.User.ActiveCharacter.Name,
                Serial = 123,
                SessionAge = 123,
            }.CreatePacket());

            packet.Sender.Send(new WeatherAnswer()
            {
                CurrentWeather = WeatherAnswer.Weather.Rain
            }.CreatePacket());
            
            packet.Sender.Send(new ChatMessageAnswer()
            {
                MessageType = Array.ConvertAll(new char[16], v => (char) 99).ToString(),
                SenderCharacterName = "Server",
                Message = "Server powered by DCNC - GigaToni",
            }.CreatePacket());
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

        [Packet(Packets.CmdFirstPosition)] // TODO: Actual position and not just dummies
        public static void FirstPosition(Packet packet)
        {
            packet.Sender.Send(new FirstPositionAnswer()
            {
                City = packet.Sender.User.ActiveCharacter.City,
                LastChannel = packet.Sender.User.ActiveCharacter.LastChannel,
                PositionX = packet.Sender.User.ActiveCharacter.PositionX,
                PositionY = packet.Sender.User.ActiveCharacter.PositionY,
                PositionZ = packet.Sender.User.ActiveCharacter.PositionZ,
                Rotation = packet.Sender.User.ActiveCharacter.Rotation,
                PositionState = packet.Sender.User.ActiveCharacter.posState
            }.CreatePacket());
        }

        [Packet(Packets.CmdAreaList)] // TODO: Actual areas and not just dummies
        public static void AreaList(Packet packet)
        {
            //if ( *(_BYTE *)&BS_Global::ContentsFlag.Main.0 >> 8 )
            /*var ack = new Packet(789); // GateList
            ack.Writer.Write(1); // Gate Count
            ack.Writer.Write(0); // Current
            ack.Writer.Write(3000); // Max
            ack.Writer.Write((short)0); // ???????
            packet.Sender.Send(ack);
            
            // Missing: BS_PktChannelOwnershipListAck
            */

            var ack = new AreaListAnswer
            {
                Areas = new Area[10]
            };
            
            for (var i = 0; i < 10; i++)
            {
                ack.Areas[i].AreaId = i;
                ack.Areas[i].CurrentPlayers = 0;
                ack.Areas[i].MaxPlayers = 100;
                ack.Areas[i].ChannelState = 1;
                ack.Areas[i].Tax = 0.0f;
                ack.Areas[i].TeamId = 0;
                ack.Areas[i].TeamMarkId = 0;
                ack.Areas[i].TeamName = "Staff";
                ack.Areas[i].Ranking = 0;
                ack.Areas[i].Point = 0;
                ack.Areas[i].WinCount = 0;
                ack.Areas[i].MemberCount = 20;
                ack.Areas[i].OwnerId = 1;
                ack.Areas[i].OwnerName = "Admin";
                ack.Areas[i].TotalExp = 0;
            }
            packet.Sender.Send(ack.CreatePacket());
            /*
                lpAck->AreaNum = 10;
          for ( k = 0; k < 10; ++k )
          {
            lpAck->m_Area[k].AreaId = k;
            lpAck->m_Area[k].Current = 0;
            lpAck->m_Area[k].Open = g_battleBoard.m_lobby[k].m_ChannelState;
            XiSmpTeamInfo::Init(&lpAck->m_Area[k].OwnTeam);
            lpAck->m_Area[k].AccTax = (double)g_battleBoard.m_lobby[k].m_teamAccTax;
            BS_CBattleLobby::GetMasterTeam(&g_battleBoard.m_lobby[k], &result);
            v18 = 7;
            if ( result.m_pObj )
              XiSmpTeamInfo::FillFrom(&lpAck->m_Area[k].OwnTeam, (XiStrTeamInfo *)&result.m_pObj->TeamId);
            v18 = -1;
            if ( result.m_pObj )
              XiCsTeam::Release(result.m_pObj);
          } 
                */

            /* Pulled from Rice:
            // client calls 2 functions (not using any packet data), returns  137 * (*(_DWORD *)(pktBuf + 2) - 1) + 143;
            var ack = new RicePacket(781);
            ack.Writer.Write(1);
            ack.Writer.Write(new byte[137]);
            packet.Sender.Send(ack);
            */
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
            var team = TeamModel.Retrieve(GameServer.Instance.Database.Connection, character.Tid);
            character.TeamId = team.TeamId;
            character.TeamName = team.TeamName;
            character.TeamMarkId = team.TeamMarkId;
            character.TeamCloseDate = (int) team.CloseDate;
            character.TeamRank = 1;
            var user = AccountModel.Retrieve(GameServer.Instance.Database.Connection, character.Uid);

            packet.Sender.User = user;
            packet.Sender.User.ActiveCharacterId = character.Cid;
            packet.Sender.User.ActiveCharacter = character;
            packet.Sender.User.ActiveCarId = character.CurrentCarId;
            packet.Sender.User.ActiveTeam = team;
            packet.Sender.User.Characters = CharacterModel.RetrieveUser(GameServer.Instance.Database.Connection, user.UID);

            var vehicles = VehicleModel.Retrieve(GameServer.Instance.Database.Connection, character.Cid);

            packet.Sender.User.ActiveCar = vehicles.Find(vehicle => vehicle.CarID == character.CurrentCarId);
            var ack = new LoadCharThreadAnswer()
            {
                ServerId = 0,
                ServerStartTime = 0,
                Character = character,
                Vehicles = vehicles.ToArray(),
                CurrentCarId = character.CurrentCarId,
            };
            packet.Sender.Send(ack.CreatePacket());

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
        public static void ItemList(Packet packet) // TODO: Send actual data
        {
            var ack = new ItemListAnswer
            {
                Items = new[]
                {
                    new XiStrMyItemMod
                    {
                        MyItem = new Item()
                        {
                            InvenIdx = 0,
                            TableIdx = 50,
                        }
                    }
                }
            };

            packet.Sender.Send(ack.CreatePacket());
        }

        [Packet(Packets.CmdVisualItemList)]
        public static void VisualItemList(Packet packet)
        {
            var ack = new Packet(Packets.VisualItemListAck);
            ack.Writer.Write(262144); // ListUpdate (262144 = First packet from list queue, 262145 = sequential)
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
                if (client.User.ActiveCharacter != null && client.User.ActiveCarId == serial)
                {
                    var character = client.User.ActiveCharacter;
                    var playerInfo = new XiPlayerInfo()
                    {
                        CharacterName = character.Name,
                        Serial = (ushort)client.User.ActiveCarId, // TODO: Verify if the serial is the same as carid
                        Age = 0
                    };
                    var res = new Packet(Packets.PlayerInfoOldAck);
                    res.Writer.Write(1); // Result
                    res.Writer.Write(playerInfo);
                    packet.Sender.Send(res);
                    break;
                }
            }
            #if !DEBUG
            Log.Unimplemented("Missing info for PlayerInfoReq");
            #endif
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