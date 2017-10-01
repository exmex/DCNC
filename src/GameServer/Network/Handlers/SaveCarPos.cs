using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class SaveCarPos
    {
        /*
        
        */
        [Packet(Packets.CmdSaveCarPos)]
        public static void Handle(Packet packet)
        {
            var saveCar = new SaveCarPosPacket(packet);

            packet.Sender.User.ActiveCharacter.LastChannel = saveCar.ChannelId;
            packet.Sender.User.ActiveCharacter.City = saveCar.CityId;
            packet.Sender.User.ActiveCharacter.Position = saveCar.Position;
            packet.Sender.User.ActiveCharacter.PosState = saveCar.PosState;

            CharacterModel.Update(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacter);
        }
    }
}