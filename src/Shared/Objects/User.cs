using System;
using System.Collections.Generic;
using System.Data.Common;
using Shared.Database;

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
        public uint ActiveCarId;
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

        public bool GMFlag;

        public List<Character> Characters;
        public string CreateIp;
        public string Name;
        public string Password;
        public string Salt;
        public UserStatus Status;
        public uint Ticket;
        public ulong UID;

        public static User Empty => new User {Status = UserStatus.Normal};

        public static User ReadFromDb(DbDataReader reader)
        {
            return new User
            {
                UID = Convert.ToUInt64(reader["UID"]),
                Name = reader["Username"] as string,
                Password = reader["Password"] as string,
                Salt = reader["Salt"] as string,
                Permission = Convert.ToInt32(reader["Permission"]),
                Ticket = Convert.ToUInt32(reader["Ticket"]),
                Status = (UserStatus) Convert.ToByte(reader["Status"]),
                CreateIp = reader["CreateIP"] as string,
                ActiveCharacterId = Convert.ToUInt64(reader["LastActiveChar"]),
            };
        }

        public void WriteToDb(ref UpdateCommand cmd)
        {
            cmd.Set("Status", (byte)Status);
            cmd.Set("Permission", Permission);
        }
    }
}