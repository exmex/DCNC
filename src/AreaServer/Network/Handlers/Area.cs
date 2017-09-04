using Shared.Models;
using Shared.Network;
using Shared.Network.AreaServer;
using Shared.Objects;
using Shared.Util;

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

            // TODO: Figure out a better way to get the character name. This is unsafe!
            packet.Sender.User = new User
            {
                ActiveCharacter = new Character()
                {
                    Name = enterAreaPacket.CharacterName,
                }
            };

            packet.Sender.Send(new EnterAreaAnswer
            {
                LocalTime = enterAreaPacket.LocalTime,
                AreaId = enterAreaPacket.AreaId
            }.CreatePacket());
        }

        [Packet(Packets.CmdMoveVehicle)]
        public static void MoveVehicle(Packet packet)
        {
            var serial = packet.Reader.ReadUInt16();
            var movement = packet.Reader.ReadBytes(112);

            var move = new Packet(Packets.CmdMoveVehicle); // 114 total length
            move.Writer.Write(serial); // TODO: Use server-side serial!
            move.Writer.Write(movement);

            AreaServer.Instance.Server.Broadcast(move);
        }
    }
}