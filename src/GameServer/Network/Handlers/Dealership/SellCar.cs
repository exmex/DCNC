using System.Linq;
using Shared;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Util;

namespace GameServer.Network.Handlers.Dealership
{
    public class SellCar
    {
        [Packet(Packets.CmdSellCar)]
        public static void Handle(Packet packet)
        {
            var character = packet.Sender.User.ActiveCharacter;
            
            var charName = packet.Reader.ReadUnicodeStatic(21);
            var vehicleId = packet.Reader.ReadUInt32();

            if (charName != character.Name)
            {
                Log.Error("User tried to cheat with SellCar! (charName != character.Name)");
                packet.Sender.KillConnection("Hack attempt blocked.");
                return;
            }
            
            if (vehicleId == character.ActiveVehicleId)
            {
                Log.Error("User tried to cheat with SellCar! (vehicleId == character.ActiveVehicleId)");
                packet.Sender.KillConnection("Hack attempt blocked.");
                return;
            }
            
            var vehicle = character.GarageVehicles.FirstOrDefault(veh => veh.CarId == vehicleId);
            if (vehicle == null)
            {
                Log.Error("User tried to sell a vehicle he doesn't own!");
                packet.Sender.KillConnection("Hack attempt blocked.");
                return;
            }
            
            var vehicleData = ServerMain.Vehicles.Find(veh =>
            {
                uint uniqueId;
                if (uint.TryParse(veh.UniqueId, out uniqueId))
                    return uniqueId == vehicleId;
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
            int price;
            if (!int.TryParse(vehicleUpgrade.Sell, out price))
            {
                Log.Error("vehicleData.Upgrades[vehicleGrade].Sell not int!");
                packet.Sender.SendError("Failed to sell the car.");
                return;
            }

            if (!VehicleModel.Remove(GameServer.Instance.Database.Connection, vehicleId))
            {
                Log.Error("Couldn't remove vehicle from DB");
                packet.Sender.SendError("Failed to sell the car.");
                return;
            }
            character.GarageVehicles.Remove(vehicle);
            
            character.MitoMoney += price;
            CharacterModel.Update(GameServer.Instance.Database.Connection, character);
            
            packet.Sender.Send(new SellCarAnswer()
            {
                CarId = (int)vehicleId,
                SellPrice = price
            }.CreatePacket());
            
            character.FlushItemModBuffer(packet.Sender);
        }
    }
}