using System.IO;
using Shared.Util;

namespace Shared.Network.LobbyServer
{
    public class CheckCharacterNameAnswerPacket : OutPacket
    {
        // Availability. true = Available, false = Unavailable.
        public bool Availability;

        public string CharacterName;

        public CheckCharacterNameAnswerPacket()
        {
            Availability = false;
            CharacterName = "";
        }

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CheckCharNameAck);
        }

        public override byte[] GetBytes()
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