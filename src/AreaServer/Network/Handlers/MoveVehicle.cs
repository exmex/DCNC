using System.Linq;
using Shared.Network;

namespace AreaServer.Network.Handlers
{
    public class MoveVehicle
    {
        [Packet(Packets.CmdMoveVehicle)]
        public static void Handle(Packet packet)
        {
            var vehicleSerial = packet.Reader.ReadUInt16();
            var movement = packet.Reader.ReadBytes(112);
            
            var validSerial = DefaultServer.ActiveSerials.FirstOrDefault(pair => pair.Value == packet.Sender.User);
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