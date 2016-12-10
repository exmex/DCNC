using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Models;
using Shared.Network;
using Shared.Objects;

namespace GameServer.Network.Handlers
{
    class Joining
    {
        /*
        02 00 00 00 00 00 00 00 61 00 64 00 6D 00 69 00  · · · · · · · · a · d · m · i · 
        6E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  n · · · · · · · · · · · · · · · 
        00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · · 
        00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · · 
        00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · · 
        00 00 00 00 00 00 00 00 72 00 61 00 00 00 00 00  · · · · · · · · r · a · · · · · 
        00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · · 
        00 00 00 00 00 00  · · · · · · 
        */
        [Packet(Packets.CmdCheckInGame)]
        public static void CheckInGame(Packet packet)
        {
            var ack = new Packet(Packets.CheckInGameAck);
            ack.Writer.Write((uint)1); // Result
            packet.Sender.Send(ack);
        }

        [Packet(Packets.CmdJoinChannel)]
        public static void JoinChannel(Packet packet)
        {
            var ack = new Packet(Packets.JoinChannelAck);
            ack.Writer.WriteUnicodeStatic("speeding", 10); // ChannelName
            ack.Writer.WriteUnicodeStatic("charName", 16); // CharName

            ack.Writer.Write((ushort)123); // Serial
            ack.Writer.Write((ushort)123); // Session Age

            packet.Sender.Send(ack);

            /*
            enum $38636D0EA7AD20B267BDBB95270A9F80
            {
              FINE = 0x0,
              CLOUDY = 0x1,
              FOGGY = 0x2,
              RAIN = 0x3,
              SUNSET = 0x4,
            };
            */
            ack = new Packet(Packets.WeatherAck);
            ack.Writer.Write(3); // RAIN
            packet.Sender.Send(ack);
        }

        [Packet(Packets.CmdJoinArea)]
        public static void JoinArea(Packet packet)
        {
            var areaId = packet.Reader.ReadUInt32(); // AreaID
            var ack = new Packet(Packets.JoinAreaAck);
            ack.Writer.Write(areaId); // AreaId
            ack.Writer.Write(1); // Result
            packet.Sender.Send(ack);
        }

        [Packet(Packets.CmdFirstPosition)] // TODO: Actual position and not just dummies
        public static void FirstPosition(Packet packet)
        {
            var ack = new Packet(Packets.FirstPositionAck);

            ack.Writer.Write(packet.Sender.User.ActiveCharacter.City); // City ID
            ack.Writer.Write(1); // Channel ID
            ack.Writer.Write(packet.Sender.User.ActiveCharacter.PositionX); // x
            ack.Writer.Write(packet.Sender.User.ActiveCharacter.PositionY); // y
            ack.Writer.Write(packet.Sender.User.ActiveCharacter.PositionZ); // z
            ack.Writer.Write(packet.Sender.User.ActiveCharacter.Rotation); // w
            ack.Writer.Write(packet.Sender.User.ActiveCharacter.posState); // PosState

            packet.Sender.Send(ack);
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

            var ack = new Packet(Packets.CmdAreaList+1);
            ack.Writer.Write((uint)10);
            for (var k = 0; k < 10; ++k)
            {
                ack.Writer.Write(k); // AreaId
                ack.Writer.Write(0); // Current?
                ack.Writer.Write(100); // Max?
                ack.Writer.Write(1); // ChannelState
                ack.Writer.Write((float)0); // Tax?

                ack.Writer.Write((long)0); // teamID?
                ack.Writer.Write((long)0); // teamMarkID
                ack.Writer.WriteUnicodeStatic("Staff", 13); // TeamName
                ack.Writer.Write((uint)0); // Ranking
                ack.Writer.Write((uint)0); // Point
                ack.Writer.Write((uint)0); // WinCnt
                ack.Writer.Write(20); // Membercnt
                ack.Writer.Write((long)1); // OwnerId
                ack.Writer.WriteUnicodeStatic("Administrator", 21); // OwnerName
                ack.Writer.Write((long)0); // TotalExp
                ack.Writer.Write((long)0); // ????????
            }
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
            string characterName = packet.Reader.ReadUnicode();
            uint serial = packet.Reader.ReadUInt32();

            var character = CharacterModel.Retrieve(GameServer.Instance.Database.Connection, characterName);
            var team = TeamModel.Retrieve(GameServer.Instance.Database.Connection, character.Tid);
            character.TeamId = team.TeamId;
            character.TeamName = team.TeamName;
            character.TeamMarkId = team.TeamMarkId;
            character.TeamCloseDate = (int)team.CloseDate;
            character.TeamRank = 1;
            var user = AccountModel.Retrieve(GameServer.Instance.Database.Connection, character.Uid);

            packet.Sender.User = user;
            packet.Sender.User.ActiveCharacterId = character.Uid;
            packet.Sender.User.ActiveCharacter = CharacterModel.RetrieveOne(GameServer.Instance.Database.Connection, character.Uid);
            packet.Sender.User.ActiveCarId = character.CurrentCarId;
            packet.Sender.User.ActiveTeam = team;
            packet.Sender.User.Characters = CharacterModel.Retrieve(GameServer.Instance.Database.Connection, user.UID);

            var vehicles = VehicleModel.Retrieve(GameServer.Instance.Database.Connection, character.Cid);

            var ack = new Packet(Packets.LoadCharThreadAck);

            ack.Writer.Write((uint)0); // ServerId
            ack.Writer.Write((uint)0); // ServerStartTime

            // Character
            character.Serialize(ack.Writer);

            ack.Writer.Write((uint)vehicles.Count);

            foreach (var vehicle in vehicles)
            {
                if (vehicle.CarID == character.CurrentCarId)
                    packet.Sender.User.ActiveCar = vehicle;
                ack.Writer.Write(vehicle.CarID);
                ack.Writer.Write(vehicle.CarType);
                ack.Writer.Write(vehicle.BaseColor);
                ack.Writer.Write(vehicle.Grade);
                ack.Writer.Write(vehicle.SlotType);
                ack.Writer.Write(vehicle.AuctionCnt);
                ack.Writer.Write(vehicle.Mitron);
                ack.Writer.Write(vehicle.Kmh);

                ack.Writer.Write(vehicle.Color);
                ack.Writer.Write(vehicle.Color2);
                ack.Writer.Write(vehicle.MitronCapacity);
                ack.Writer.Write(vehicle.MitronEfficiency);
                ack.Writer.Write(vehicle.AuctionOn);
                ack.Writer.Write(vehicle.SBBOn);
            }
            packet.Sender.Send(ack);

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
        }

        /*
        00 00 00 00  · · · · 
        * uint GivePostIdx
        */
        [Packet(Packets.CmdMyQuestList)]
        public static void MyQuestList(Packet packet) // TODO: Send actual data
        {
            var ack = new Packet(Packets.MyQuestListAck);
            //ack.Writer.Write(0); // Quest num
            ack.Writer.Write(1); // Quest num


            ack.Writer.Write((uint)0);
            ack.Writer.Write((uint)2);
            ack.Writer.Write((uint)0);
            ack.Writer.Write((ushort)0);
            /*
              unsigned int TableIdx;
              unsigned int State;
              unsigned int PlaceIdx;
              unsigned __int16 FailNum;
            */
            packet.Sender.Send(ack);
        }

        [Packet(Packets.CmdItemList)]
        public static void ItemList(Packet packet) // TODO: Send actual data
        {
            var ack = new Packet(Packets.ItemListAck);
            ack.Writer.Write((uint)0x40000); // WHAT THE? unsigned int ListUpdate;
            //ack.Writer.Write((uint)0); // Count
            ack.Writer.Write((uint)0); // Count
            // 52 bytes of data for each item
            /*
            ack.Writer.Write((uint)1); // CarID
            ack.Writer.Write((ushort)1); // State
            ack.Writer.Write((ushort)1); // Slot
            ack.Writer.Write((uint)1); // StateVar
            ack.Writer.Write(1); // StackNum
            ack.Writer.Write(0); // Random

            ack.Writer.Write((uint)0); // AssistA
            ack.Writer.Write((uint)0); // AssistB
            ack.Writer.Write((uint)0); // Box
            ack.Writer.Write((uint)0); // Belonging
            ack.Writer.Write(0); // Upgrade
            ack.Writer.Write(0); // UpgradePoint
            ack.Writer.Write((uint)0); // ExpireTick
            ack.Writer.Write((uint)1); // TableIdx
            ack.Writer.Write((uint)0); // InvenIdx
            ack.Writer.Write(new byte[46]); // 94
            */

            packet.Sender.Send(ack);
        }
    }
}
