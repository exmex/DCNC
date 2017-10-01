using Shared;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Util;

namespace GameServer.Network.Handlers.Dealership
{
    public class BuyCar
    {
        [Packet(Packets.CmdBuyCar)]
        public static void Handle(Packet packet)
        {
            var character = packet.Sender.User.ActiveCharacter;
            // Save current car.
            VehicleModel.Update(GameServer.Instance.Database.Connection, character.ActiveCar);

            var buyCarPacket = new BuyCarPacket(packet);
            var price = 10; //XiVehicleTable::GetDefaultVehicleAbility(v14, v13, &Info) //Failed to purchase the car.
            var vehicleData = ServerMain.Vehicles.Find(vehicle =>
            {
                uint uniqueId;
                if (uint.TryParse(vehicle.UniqueId, out uniqueId))
                    return uniqueId == buyCarPacket.CarType;
                return false;
            });
            
            if (vehicleData == null)
            {
                Log.Error("vehicleData == null");
                packet.Sender.SendError("Failed to purchase the car.");
                return;
            }

            if (vehicleData.Upgrades.Count == 0)
            {
                Log.Error("vehicleData.Upgrades.Count == 0");
                packet.Sender.SendError("Failed to purchase the car.");
                return;
            }

            int vehicleGrade;
            if (!int.TryParse(vehicleData.Grade, out vehicleGrade))
            {
                Log.Error("vehicleData.Grade not int!");
                return;
            }
            var vehicleUpgrade = vehicleData.Upgrades[vehicleGrade];
            
            if (!int.TryParse(vehicleUpgrade.Price, out price))
            {
                Log.Error("vehicleData.Upgrades[vehicleGrade].Price not int!");
                packet.Sender.SendError("Failed to purchase the car.");
                return;
            }

            if (character.MitoMoney < price)
            {
                packet.Sender.SendError("Insufficient funds.");
                return;
            }

            var vehicleCount = VehicleModel.RetrieveCount(GameServer.Instance.Database.Connection,
                character.Id);
            if (vehicleCount >= (character.GarageLevel + 1) * 8)
            {
                packet.Sender.SendError(((char) 87u).ToString());
                return;

                /*
                if ( XiCsCharInfo::GetGarageSpace(pCharInfo) <= 0 )
                  {
                    PacketSend::Send_Error(lpDispatch->m_pSession, &off_6B8AD0);
                    return 52;
                  }
                */
            }

            character.MitoMoney -= price;
            CharacterModel.Update(GameServer.Instance.Database.Connection, character);

            character.ActiveCar = new Vehicle();
            character.ActiveCar.CarType = buyCarPacket.CarType;
            character.ActiveCar.BaseColor = 0;
            uint.TryParse(vehicleData.Grade, out character.ActiveCar.Grade);
            character.ActiveCar.SlotType = 0;
            character.ActiveCar.AuctionCnt = 0;
            character.ActiveCar.Mitron = 100.0f;
            character.ActiveCar.Kmh = 0.0f;
            character.ActiveCar.Color = buyCarPacket.Color;
            float.TryParse(vehicleUpgrade.Capacity, out character.ActiveCar.MitronCapacity);
            float.TryParse(vehicleUpgrade.Efficiency, out character.ActiveCar.MitronEfficiency);
            character.ActiveCar.MitronCapacity = 0.0f;
            character.ActiveCar.MitronEfficiency = 0.0f;
            character.ActiveCar.AuctionOn = false;

            // Save newly bought vehicle
            var carId = VehicleModel.Create(GameServer.Instance.Database.Connection, character.ActiveCar,
                character.Id);
            character.ActiveCar.CarID = (uint) carId;
            CharacterModel.Update(GameServer.Instance.Database.Connection, character);

            // TODO: Send actual data.
            var ack = new StatUpdateAnswer()
            {
                StatisticInfo = new XiStrStatInfo(),
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
            };
            
            if (!int.TryParse(vehicleData.Acceleration, out ack.StatisticInfo.BasedAccel))
                Log.Error("Acceleration parse error.");
            if(!int.TryParse(vehicleData.Boost, out ack.StatisticInfo.BasedBoost))
                Log.Error("Boost parse error.");
            if(!int.TryParse(vehicleData.Crash, out ack.StatisticInfo.BasedCrash))
                Log.Error("Crash parse error.");
            if(!int.TryParse(vehicleData.Speed, out ack.StatisticInfo.BasedSpeed))
                Log.Error("Speed parse error.");
            
            ack.StatisticInfo.CharAccel = 0;
            ack.StatisticInfo.CharBoost = 0;
            ack.StatisticInfo.CharCrash = 0;
            ack.StatisticInfo.CharSpeed = 0;
            ack.StatisticInfo.EquipAccel = 0;
            ack.StatisticInfo.EquipBoost = 0;
            ack.StatisticInfo.EquipCrash = 0;
            ack.StatisticInfo.EquipSpeed = 0;
            ack.StatisticInfo.ItemUseAccel = 0;
            ack.StatisticInfo.ItemUseBoost = 0;
            ack.StatisticInfo.ItemUseCrash = 0;
            ack.StatisticInfo.ItemUseSpeed = 0;
            ack.StatisticInfo.TotalAccel = 0;
            ack.StatisticInfo.TotalBoost = 0;
            ack.StatisticInfo.TotalCrash = 0;
            ack.StatisticInfo.TotalSpeed = 0;
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

            var carInfo = new XiStrCarInfo()
            {
                CarID = character.ActiveCar.CarID,
                CarType = buyCarPacket.CarType,
                BaseColor = character.ActiveCar.BaseColor,
                Grade = character.ActiveCar.Grade,
                SlotType = character.ActiveCar.SlotType,
                AuctionCnt = character.ActiveCar.AuctionCnt,
                Mitron = character.ActiveCar.Mitron,
                Kmh = character.ActiveCar.Kmh,
                Color = buyCarPacket.Color,
                MitronCapacity = character.ActiveCar.MitronCapacity,
                MitronEfficiency = character.ActiveCar.MitronEfficiency,
                AuctionOn = character.ActiveCar.AuctionOn,
            };

            packet.Sender.Send(new VisualUpdateAnswer()
            {
                Serial = packet.Sender.User.VehicleSerial,
                Age = 0,
                CarId = character.ActiveCar.CarID,
                CarInfo = carInfo
            }.CreatePacket());

            packet.Sender.Send(new BuyCarAnswer
            {
                CarInfo = carInfo,
                Price = price
            }.CreatePacket());
        }
    }
}