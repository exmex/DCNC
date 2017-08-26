using System.IO;
using Shared.Util;

namespace Shared.Network.LobbyServer
{
    public class DeleteCharacterAnswerPacket
    {
        public string CharacterName;

        /// <summary>
        ///     Sends the answer packet.
        /// </summary>
        /// <param name="client">The client to send the packet to.</param>
        public void Send(Client client)
        {
            var ack = new Packet(Packets.DeleteCharAck);
            ack.Writer.Write(GetBytes());
            //ack.Writer.WriteUnicodeStatic(CharacterName, 21);
            client.Send(ack);
        }

        public byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.WriteUnicodeStatic(CharacterName, 21);
                }
                return ms.GetBuffer();
            }
        }
    }
}