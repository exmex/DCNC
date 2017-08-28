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
        public Vehicle ActiveCar;
        public int ActiveCarId;
        public Character ActiveCharacter;
        public ulong ActiveCharacterId;
        public Team ActiveTeam;

        /// <summary>
        ///     The permission flags
        ///     Valid values:
        ///     0x8000 => Administrator
        ///     0x4000 => Power User
        ///     0x2000 => Remote Client User
        ///     0x1000 => Developer
        ///     0x0 => User
        /// </summary>
        public int Permission;

        public List<Character> Characters;
        public string CreateIp;
        public string Name;
        public string Password;
        public string Salt;
        public UserStatus Status;
        public uint Ticket;
        public ulong UID;

        public static User Empty => new User {Status = UserStatus.Normal};
    }
}