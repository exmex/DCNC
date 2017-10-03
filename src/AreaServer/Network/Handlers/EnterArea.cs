using Shared.Models;
using Shared.Network;
using Shared.Network.AreaServer;
using Shared.Util;

namespace AreaServer.Network.Handlers
{
    public static class EnterArea
    {
        [Packet(Packets.CmdEnterArea)]
        public static void Handle(Packet packet)
        {
            var enterAreaPacket = new EnterAreaPacket(packet);
            
            if (packet.Sender.User == null || packet.Sender.User.VehicleSerial != enterAreaPacket.VehicleSerial)
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

                if (DefaultServer.ActiveSerials.ContainsKey(enterAreaPacket.VehicleSerial))
                {
                    if (packet.Sender.User.VehicleSerial != enterAreaPacket.VehicleSerial)
                    {
                        packet.Sender.KillConnection($"[{packet.Sender.User.VehicleSerial} vs {enterAreaPacket.VehicleSerial}] Still wrong user.");
                        return;
                    }

                }else
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