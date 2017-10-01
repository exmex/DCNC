using Shared.Models;
using Shared.Network;
using Shared.Network.AreaServer;

namespace AreaServer.Network.Handlers
{
    public static class EnterArea
    {
        [Packet(Packets.CmdEnterArea)]
        public static void Handle(Packet packet)
        {
            var enterAreaPacket = new EnterAreaPacket(packet);
            
            if (packet.Sender.User == null)
            {
                var character = CharacterModel.Retrieve(AreaServer.Instance.Database.Connection, enterAreaPacket.CharacterName);
                if (character == null)
                {
                    packet.Sender.KillConnection("Invalid charactername");
                    return;
                }
                
                var account = AccountModel.RetrieveFromSerial(AreaServer.Instance.Database.Connection, character.Uid, enterAreaPacket.VehicleSerial);
                if (account == null)
                {
                    packet.Sender.KillConnection("Invalid serial");
                    return;
                }
                
                packet.Sender.User = account;
                packet.Sender.User.ActiveCharacter = character;
                
                DefaultServer.ActiveSerials.Add(enterAreaPacket.VehicleSerial, packet.Sender.User);
            }

            packet.Sender.Send(new EnterAreaAnswer
            {
                LocalTime = enterAreaPacket.LocalTime,
                AreaId = enterAreaPacket.AreaId
            }.CreatePacket());
        }
    }
}