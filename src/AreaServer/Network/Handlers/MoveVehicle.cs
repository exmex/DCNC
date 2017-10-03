using System.Linq;
using Shared.Network;
using Shared.Network.AreaServer;

namespace AreaServer.Network.Handlers
{
    public class MoveVehicle
    {
        /// <summary>
        /// Ack size: 110
        /// </summary>
        /// <param name="packet"></param>
        [Packet(Packets.CmdMoveVehicle)]
        public static void Handle(Packet packet)
        {
            var vehicleSerial = packet.Reader.ReadUInt16();
            //packet.Reader.ReadUInt16(); // Age.
            var movement = packet.Reader.ReadBytes(112);
            /*var moveVehiclePkt = new MoveVehiclePacket(packet);
            
            var validSerial = DefaultServer.ActiveSerials.FirstOrDefault(pair => pair.Value == packet.Sender.User);
            if (validSerial.Value == null || validSerial.Key != moveVehiclePkt.VehicleSerial)
            {
                packet.Sender.KillConnection("Vehicle serial didn't match!");
                return;
            }

            var ack = new MoveVehicleAnswer()
            {
                
            }
            
            // TODO: Make plausability check?*/
            
            var move = new Packet(Packets.CmdMoveVehicle); // 114 total length
            move.Writer.Write(vehicleSerial);
            move.Writer.Write(movement);

            AreaServer.Instance.Server.Broadcast(move, packet.Sender);
        }
    }
}