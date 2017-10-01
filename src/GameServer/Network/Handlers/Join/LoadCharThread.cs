using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;

namespace GameServer.Network.Handlers.Join
{
    public class LoadCharThread
    {
        /*
        41 00 64 00 6D 00 69 00 6E 00 69 00 73 00 74 00  A · d · m · i · n · i · s · t ·
        72 00 61 00 74 00 6F 00 72 00 00 00 00 00 00 00  r · a · t · o · r · · · · · · ·
        00 00 00 00 00 00 00 00 00 00 01 00 00 00  · · · · · · · · · · · · · · 
        */
        [Packet(Packets.CmdLoadCharThread)]
        public static void Handle(Packet packet)
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
    }
}