using System;
using System.Collections.Generic;
using System.Data.Common;
using Shared.Database;
using Shared.Models;
using Shared.Util;

namespace Shared.Objects
{
    public enum UserStatus : byte
    {
        Invalid = 0,
        Normal = 1,
        Banned = 2,
        Muted = 3
    }

    public enum UserPermission : int
    {
        Administrator = 0x8000, // 32768
        PowerUser = 0x4000, // 16384
        RemoteClientUser = 0x2000, // 8192
        Developer = 0x1000, // 4096
        User = 0x0,
    }

    public class User
    {
        /// <summary>
        /// The Character the user is currently using
        /// </summary>
        public Character ActiveCharacter;

        /// <summary>
        /// The Character Db Id the user is currently using
        /// </summary>
        public ulong ActiveCharacterId;

        /// <summary>
        ///     The permission flags
        ///     Valid values:
        ///     0x8000 => Administrator
        ///     0x4000 => Power User
        ///     0x2000 => Remote Client User
        ///     0x1000 => Developer
        ///     0x0 => User
        /// </summary>
        public UserPermission Permission = UserPermission.User;

        /// <summary>
        /// [INTERNAL] Used for GM commands
        /// </summary>
        public bool GmFlag;

        /// <summary>
        /// Full list of all characters of this user
        /// </summary>
        public List<Character> Characters;
        
        /// <summary>
        /// IP
        /// </summary>
        public string CreateIp;

        /// <summary>
        /// The users Username
        /// </summary>
        public string Username;

        /// <summary>
        /// The users hashed password
        /// </summary>
        public string Password;

        /// <summary>
        /// The users salt hash
        /// <see cref="AccountModel.SaltSize"/>
        /// </summary>
        public string Salt;

        /// <summary>
        /// The status of the account
        /// </summary>
        public UserStatus Status;

        /// <summary>
        /// The current session ticket
        /// </summary>
        public uint Ticket;

        /// <summary>
        /// The Id in database
        /// </summary>
        public ulong Id;

        /// <summary>
        /// Unix Timestamp for temporary bans 
        /// </summary>
        public long BanValidUntil;

        public static User Empty => new User
        {
            Status = UserStatus.Invalid,
            Permission = UserPermission.User
        };

        public ushort VehicleSerial;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static uint CreateSessionTicket() => RandomProvider.Get().NextUInt32();

        /// <summary>
        /// Hashed and then checks the specified password with the value from DB
        /// </summary>
        /// <param name="plainTextPassword">The password in plaintext</param>
        /// <returns>true if password is correct, false otherwise</returns>
        public bool CheckPassword(string plainTextPassword)
        {
            var passwordHashed = Util.Password.GenerateSaltedHash(plainTextPassword, Salt);
            return passwordHashed == Password;
        }

        public bool IsUserBanned()
        {
            if (Status != UserStatus.Banned) return false;
            
            if (BanValidUntil == 0) // Forever
                return true;
                
            return BanValidUntil > DateTimeOffset.Now.ToUnixTimeSeconds();
        }
    }
}