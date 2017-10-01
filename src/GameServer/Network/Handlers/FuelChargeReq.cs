using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class FuelChargeReq
    {
        /// <summary>
        /// Handles the fuel charge packet
        /// TODO: Move Sale price to a server settings
        /// </summary>
        /// <param name="packet"></param>
        [Packet(Packets.CmdFuelChargeReq)]
        public static void Handle(Packet packet)
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
    }
}