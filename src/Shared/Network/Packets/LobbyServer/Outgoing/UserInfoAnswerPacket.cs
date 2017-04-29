using Shared.Objects;

namespace Shared.Network.LobbyServer
{
    public class UserInfoAnswerPacket
    {
        /// <summary>
        /// The permissions the user has
        /// </summary>
        public int Permissions;

        /// <summary>
        /// The character count
        /// </summary>
        /// <remarks>Could use Characters.Length</remarks>
        public int CharacterCount;

        /// <summary>
        /// The username
        /// </summary>
        /// <remarks>Always 18</remarks>
        public string Username;

        /// <summary>
        /// The characters the user has
        /// </summary>
        public Character[] Characters;
        
        public UserInfoAnswerPacket()
        {
            Permissions = 0;
            CharacterCount = 0;
            Username = "";
            Characters = new Character[0];
        }

        /// <summary>
        /// Sends the user info answer packet.
        /// </summary>
        /// <param name="packetId">The packet identifier.</param>
        /// <param name="client">The client to send the packet to.</param>
        public void Send(ushort packetId, Client client)
        {
            Packet packet = new Packet(packetId);

            packet.Writer.Write(Permissions);
            packet.Writer.Write(CharacterCount);
            packet.Writer.WriteUnicodeStatic(Username, 18);
            packet.Writer.Write((long)0);
            packet.Writer.Write((long)0);
            packet.Writer.Write((long)0);
            packet.Writer.Write(0);

            foreach (Character character in Characters)
            {
                packet.Writer.WriteUnicodeStatic(character.Name, 21);
                packet.Writer.Write(character.Cid);
                packet.Writer.Write((int)character.Avatar);
                packet.Writer.Write((int)character.Level);
                packet.Writer.Write(character.CurrentCarId);
                packet.Writer.Write(character.ActiveCar.CarType);
                packet.Writer.Write(character.ActiveCar.BaseColor);
                packet.Writer.Write(character.CreationDate);
                packet.Writer.Write(character.Tid);
                packet.Writer.Write(character.TeamMarkId);
                packet.Writer.WriteUnicodeStatic(character.TeamName, 13);
                packet.Writer.Write((short)1);
                packet.Writer.Write((short)-1);
            }

            client.Send(packet);
        }
    }
}
