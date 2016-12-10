using System.Collections.Generic;

namespace Shared.Objects
{
    public enum UserStatus : byte
    {
        Invalid = 0,
        Normal = 1,
        Banned = 2,
        Muted = 3
    }

    public class User
    {
        public ulong UID;
        public string Name;
        public string Password;
        public string Salt;
        public uint Ticket;
        public UserStatus Status;
        public string CreateIp;
        public Character ActiveCharacter;
        public ulong ActiveCharacterId;
        public Vehicle ActiveCar;
        public int ActiveCarId;
        public Team ActiveTeam;

        public List<Character> Characters;

        public User()
        {
        }

        public static User Empty => new User() { Status = UserStatus.Normal };
    }
}
