using Shared.Network;
using Shared.Util;

namespace AreaServer.Network.Handlers
{
    public static class Chat
    {
        /// <summary>
        /// </summary>
        /// <param name="packet"></param>
        [Packet(Packets.CmdAreaChat)]
        public static void AreaChat(Packet packet)
        {
            if (packet.Sender.User == null)
            {
                packet.Sender.KillConnection("Not loggedin!");
                return;
            }
            
            var type = packet.Reader.ReadUnicodeStatic(10);
            //var sender = packet.Reader.ReadUnicodeStatic(18);
            packet.Reader.ReadUnicodeStatic(18);
            var message = packet.Reader.ReadUnicodePrefixed();

            //string sender = packet.Sender.Player.ActiveCharacter.Name;

            var ack = new Packet(Packets.CmdAreaChat);
            ack.Writer.WriteUnicodeStatic(type, 10);
            ack.Writer.WriteUnicodeStatic(packet.Sender.User.ActiveCharacter.Name, 18);
            ack.Writer.WriteUnicode(message);

            if (type == "area")
                AreaServer.Instance.Server.Broadcast(ack);
        }
    }
}