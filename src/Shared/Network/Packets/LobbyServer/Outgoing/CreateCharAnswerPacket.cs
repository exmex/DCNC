using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.LobbyServer
{
    /// <summary>
    /// sub_53D130
    /// </summary>
    public class CreateCharAnswerPacket : OutPacket
    {
        /// <summary>
        /// The character name
        /// </summary>
        public string CharacterName;
        
        /// <summary>
        /// The Db Id of the newly created char 
        /// </summary>
        public ulong CharacterId;
        
        /// <summary>
        /// The Db Id of the newly created vehicle
        /// </summary>
        public int ActiveVehicleId;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CreateCharAck);
        }

        public override int ExpectedSize() => 56;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.WriteUnicodeStatic(CharacterName, 21);
                    bs.Write(CharacterId);
                    bs.Write(ActiveVehicleId);
                }
                return ms.ToArray();
            }
        }
    }
}