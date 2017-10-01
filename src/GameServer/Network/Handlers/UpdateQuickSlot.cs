using Shared.Network;

namespace GameServer.Network.Handlers
{
    public class UpdateQuickSlot
    {
        [Packet(Packets.CmdUpdateQuickSlot)]
        public static void Handle(Packet packet)
        {
            // TODO: actually update the quickslots.
            var slot1 = packet.Reader.ReadUInt32();
            var slot2 = packet.Reader.ReadUInt32();
        }
    }
}