using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.LobbyServer
{
    public class UserInfoAnswerPacket
    {
        /// <summary>
        ///     The character count
        /// </summary>
        /// <remarks>Could use Characters.Length</remarks>
        public int CharacterCount;

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
            CharacterCount = 0;
            Username = "";
            Characters = new Character[0];
        }

        /// <summary>
        ///     Sends the user info answer packet.
        /// </summary>
        /// <param name="packetId">The packet identifier.</param>
        /// <param name="client">The client to send the packet to.</param>
        public void Send(ushort packetId, Client client)
        {
            var packet = new Packet(packetId);
            packet.Writer.Write(GetBytes());

            /*
            packet.Writer.Write(Permissions);
            packet.Writer.Write(CharacterCount);
            packet.Writer.WriteUnicodeStatic(Username, 18);
            packet.Writer.Write((long) 0);
            packet.Writer.Write((long) 0);
            packet.Writer.Write((long) 0);
            packet.Writer.Write(0);

            foreach (var character in Characters)
            {
                packet.Writer.WriteUnicodeStatic(character.Name, 21);
                packet.Writer.Write(character.Cid);
                packet.Writer.Write((int) character.Avatar);
                packet.Writer.Write((int) character.Level);
                packet.Writer.Write(character.CurrentCarId);
                packet.Writer.Write(character.ActiveCar.CarType);
                packet.Writer.Write(character.ActiveCar.BaseColor);
                packet.Writer.Write(character.CreationDate);
                packet.Writer.Write(character.Tid);
                packet.Writer.Write(character.TeamMarkId);
                packet.Writer.WriteUnicodeStatic(character.TeamName, 13);
                packet.Writer.Write(0); // GuildType? (unsigned int nGuild;)
            }
            */

            client.Send(packet);
        }

        public byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Permissions);
                    bs.Write(CharacterCount);
                    bs.WriteUnicodeStatic(Username, 18);
                    bs.Write((long) 0);
                    bs.Write((long) 0);
                    bs.Write((long) 0);
                    bs.Write(0);

                    foreach (var character in Characters)
                    {
                        bs.WriteUnicodeStatic(character.Name, 21);
                        bs.Write(character.Cid);
                        bs.Write((int) character.Avatar);
                        bs.Write((int) character.Level);
                        bs.Write(character.CurrentCarId);
                        bs.Write(character.ActiveCar.CarType);
                        bs.Write(character.ActiveCar.BaseColor);
                        bs.Write(character.CreationDate);
                        bs.Write(character.Tid);
                        bs.Write(character.TeamMarkId);
                        bs.WriteUnicodeStatic(character.TeamName, 13);
                        bs.Write(0); // GuildType? (unsigned int nGuild;)
                    }
                }
                return ms.GetBuffer();
            }
        }
    }
}