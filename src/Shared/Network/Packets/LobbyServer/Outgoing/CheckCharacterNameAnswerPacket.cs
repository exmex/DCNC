using System.IO;
using Shared.Util;

namespace Shared.Network.LobbyServer
{
    public class CheckCharacterNameAnswerPacket
    {
        // Availability. true = Available, false = Unavailable.
        public bool Availability;

        public string CharacterName;

        /// <summary>
        ///     Sends the answer packet.
        /// </summary>
        /// <param name="client">The client to send the packet to.</param>
        public void Send(Client client)
        {
            var ack = new Packet(Packets.CheckCharNameAck);
            ack.Writer.Write(GetBytes());
            /*
            ack.Writer.WriteUnicodeStatic(CharacterName, 21);
            ack.Writer.Write(Availability);*/
            client.Send(ack);
        }

        public byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.WriteUnicodeStatic(CharacterName, 21);
                    bs.Write(Availability);
                }
                return ms.GetBuffer();
            }
        }
    }
}