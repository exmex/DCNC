using System.Linq;
using System.Security.Principal;
using Shared.Models;
using Shared.Network;
using Shared.Network.AreaServer;
using Shared.Objects;

namespace AreaServer.Network.Handlers
{
    public static class Area
    {
        [Packet(Packets.CmdAreaStatus)]
        public static void AreaStatus(Packet packet)
        {
            packet.Sender.Send(new AreaStatusAnswerPacket()
            {
                UserCount = new uint[100],
            }.CreatePacket());
        }

        [Packet(Packets.CmdEnterArea)]
        public static void EnterArea(Packet packet)
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
                
                AreaServer.Instance.Server.ActiveSerials.Add(enterAreaPacket.VehicleSerial, packet.Sender.User);
            }

            packet.Sender.Send(new EnterAreaAnswer
            {
                LocalTime = enterAreaPacket.LocalTime,
                AreaId = enterAreaPacket.AreaId
            }.CreatePacket());
        }

        [Packet(Packets.CmdMoveVehicle)]
        public static void MoveVehicle(Packet packet)
        {
            var vehicleSerial = packet.Reader.ReadUInt16();
            var movement = packet.Reader.ReadBytes(112);
            
            var validSerial = AreaServer.Instance.Server.ActiveSerials.FirstOrDefault(pair => pair.Value == packet.Sender.User);
            if (validSerial.Value == null || validSerial.Key != vehicleSerial)
            {
                packet.Sender.KillConnection("Vehicle serial didn't match!");
                return;
            }
            
            var move = new Packet(Packets.CmdMoveVehicle); // 114 total length
            move.Writer.Write(vehicleSerial);
            move.Writer.Write(movement);

            AreaServer.Instance.Server.Broadcast(move, packet.Sender);
        }
    }
}