using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.LobbyServer
{
    /// <summary>
    /// sub_53E260
    /// </summary>
    public class UserInfoAnswerPacket : OutPacket
    {
        /// <summary>
        ///     The characters the user has
        /// </summary>
        public Character[] Characters;

        /// <summary>
        ///     The permissions the user has
        /// </summary>
        public int Permissions;

        /// <summary>
        ///     The username
        /// </summary>
        /// <remarks>Always 18</remarks>
        public string Username;

        public UserInfoAnswerPacket()
        {
            Permissions = 0;
            Username = "";
            Characters = new Character[0];
        }

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.UserInfoAck);
        }

        public override int ExpectedSize() => (120*(Characters.Length-1)) + 194; // 194 => 74 + 120!! 
        
        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Permissions);
                    //bs.Write(CharacterCount);
                    bs.Write(Characters.Length);
                    bs.WriteUnicodeStatic(Username, 18);
                    bs.Write((long) 0);
                    bs.Write((long) 0);
                    bs.Write((long) 0);
                    bs.Write(0);
                    // --- 74 --- //

                    foreach (var character in Characters)
                    {
                        character.SerializeShort(bs);
                    }
                }
                return ms.ToArray();
            }
        }
    }
}