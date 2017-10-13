using System.Linq;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Util;

namespace GameServer.Network.Handlers
{
    public class SelectCar
    {
        [Packet(Packets.CmdSelectCar)]
        public static void Handle(Packet packet)
        {
            var character = packet.Sender.User.ActiveCharacter;
            
            var vehId = packet.Reader.ReadUInt32();
            var vehicle = character.GarageVehicles.FirstOrDefault(veh => veh.CarId == vehId);
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