using System;
using System.Collections.Generic;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Objects;
using Shared.Util;

namespace Shared.Models
{
    /// <summary>
    /// 
    /// </summary>
    public static class AccountModel
    {
        public const int SaltSize = 32;

        public static User GetUser(DbDataReader reader)
        {
            var user = new User();
            user.Id = Convert.ToUInt64(reader["UID"]);
            user.Username = reader["Username"] as string;
            user.Password = reader["Password"] as string;
            user.Salt = reader["Salt"] as string;
            user.Permission = Convert.ToInt32(reader["Permission"]);
            user.Ticket = Convert.ToUInt32(reader["Ticket"]);
            user.Status = (UserStatus) Convert.ToByte(reader["Status"]);
            user.CreateIp = reader["CreateIP"] as string;
            user.ActiveCharacterId = Convert.ToUInt64(reader["LastActiveChar"]);
            user.BanValidUntil = Convert.ToInt64(reader["BanValidUntil"]);
            user.VehicleSerial = Convert.ToUInt16(reader["VehicleSerial"]);
            return user;
        }

        public static int Write(User user, InsertCommand cmd)
        {
            cmd.Set("Username", user.Username);
            cmd.Set("Password", user.Password);
            cmd.Set("Salt", user.Salt);
            cmd.Set("CreateIP", user.CreateIp);
            cmd.Set("CreateDate", DateTimeOffset.Now.ToUnixTimeSeconds());
            cmd.Set("Ticket", user.Ticket);
            cmd.Set("Status", (byte)user.Status);
            cmd.Set("Permission", user.Permission);
            cmd.Set("BanValidUntil", user.BanValidUntil);
            
            return cmd.Execute();
        }

        public static int Write(User user, UpdateCommand cmd)
        {
            cmd.Set("Password", user.Password);
            cmd.Set("Salt", user.Salt);
            cmd.Set("CreateIP", user.CreateIp);
            cmd.Set("Ticket", user.Ticket);
            cmd.Set("Status", (byte)user.Status);
            cmd.Set("Permission", user.Permission);
            cmd.Set("BanValidUntil", user.BanValidUntil);
            
            return cmd.Execute();
        }

        /// <summary>
        /// Retrieves a user from it's username and password
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="username">The username</param>
        /// <param name="passwordHash">The already hashed-password</param>
        /// <returns>A new User class, null if no user was found</returns>
        public static User Retrieve(MySqlConnection dbconn, string username, string passwordHash)
        {
            var command = new MySqlCommand(
                "SELECT * FROM Users WHERE Username = @user AND Password = @pwhash", dbconn);

            command.Parameters.AddWithValue("@user", username);
            command.Parameters.AddWithValue("@pwhash", passwordHash);
            
            using (DbDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read()) return null;
                
                return GetUser(reader);
            }
        }

        /// <summary>
        /// Retrieves a user from username
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="username">The username to find</param>
        /// <returns>A new User class, null if no user was found</returns>
        public static User Retrieve(MySqlConnection dbconn, string username)
        {
            var command = new MySqlCommand("SELECT * FROM Users WHERE Username = @user", dbconn);

            command.Parameters.AddWithValue("@user", username);
            
            using (DbDataReader reader = command.ExecuteReader())
            {
                return !reader.Read() ? null : GetUser(reader);
            }
        }

        /// <summary>
        /// Retrives a user from userid
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="uid">The userid</param>
        /// <returns>A new User class, null if no user was found</returns>
        public static User Retrieve(MySqlConnection dbconn, ulong uid)
        {
            var command = new MySqlCommand("SELECT * FROM Users WHERE UID = @uid", dbconn);

            command.Parameters.AddWithValue("@uid", uid);

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read()) return null;
                return GetUser(reader);
            }
        }

        /// <summary>
        ///     Adds new account to the database.
        /// Handles hashing!
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="ip">The ip of the user</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password in plain-text</param>
        public static long CreateAccount(MySqlConnection dbconn, string ip, string username, string password)
        {
            var salt = Password.CreateSalt(SaltSize);
            password = Password.GenerateSaltedHash(password, salt);

            var userId = 0L;
            using (var cmd = new InsertCommand("INSERT INTO `Users` {0}", dbconn))
            {
                cmd.Set("Username", username);
                cmd.Set("Password", password);
                cmd.Set("Salt", salt);
                cmd.Set("Status", 1);
                cmd.Set("CreateIP", ip);
                cmd.Set("CreateDate", DateTime.Now);
                cmd.Set("Ticket", 0);

                cmd.Execute();
                userId = cmd.LastId;
            }
            return userId;
        }

        /// <summary>
        /// Checks if the specified account exists
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="username">The username</param>
        /// <returns>true if an account was found, false otherwise</returns>
        public static bool AccountExists(MySqlConnection dbconn, string username)
        {
            var mc = new MySqlCommand("SELECT `UID` FROM `Users` WHERE `Username` = @user", dbconn);
            mc.Parameters.AddWithValue("@user", username);

            using (var reader = mc.ExecuteReader())
            {
                return reader.HasRows;
            }
        }
        
        public static bool UpdateVehicleSerial(MySqlConnection dbconn, ulong userId, ushort serial)
        {
            using (var mc = new MySqlCommand(
                "UPDATE `Users` SET `VehicleSerial` = 0 WHERE `VehicleSerial` = @vehicleSerial",
                dbconn))
            {
                mc.Parameters.AddWithValue("@vehicleSerial", serial);

                mc.ExecuteNonQuery();
            }
            
            using (var mc = new MySqlCommand("UPDATE `Users` SET `VehicleSerial` = @vehicleSerial WHERE `UID` = @userId",
                dbconn))
            {

                mc.Parameters.AddWithValue("@userId", userId);
                mc.Parameters.AddWithValue("@vehicleSerial", serial);

                return mc.ExecuteNonQuery() == 1;
            }
        }
        
        /// <summary>
        ///     Sets the account password to the specified password
        ///     Handles hashing!
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password in plain-text</param>
        public static bool SetAccountPassword(MySqlConnection dbconn, string username, string password)
        {
            using (var mc =
                new MySqlCommand("UPDATE `Users` SET `Password` = @password, `Salt` = @salt WHERE `Username` = @user",
                    dbconn))
            {
                var salt = Password.CreateSalt(SaltSize);
                mc.Parameters.AddWithValue("@user", username);
                mc.Parameters.AddWithValue("@password", Password.GenerateSaltedHash(password, salt));
                mc.Parameters.AddWithValue("@salt", salt);

                return mc.ExecuteNonQuery() == 1;
            }
        }

        public static bool SetAccountStatus(MySqlConnection dbconn, string username, UserStatus status)
        {
            using (var mc =
                new MySqlCommand("UPDATE `Users` SET `Status` = @status WHERE `Username` = @user",
                    dbconn))
            {
                var salt = Password.CreateSalt(SaltSize);
                mc.Parameters.AddWithValue("@user", username);
                mc.Parameters.AddWithValue("@status", (byte)status);

                return mc.ExecuteNonQuery() == 1;
            }
        }

        /// <summary>
        ///     Sets new randomized session key for the account and returns it.
        ///     FIXME: Possible collision issue! Two users with same sessionkeys would be bad!
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="username">The account username</param>
        /// <returns>A new sessionkey</returns>
        public static uint CreateSession(MySqlConnection dbconn, string username)
        {
            using (var mc = new MySqlCommand("UPDATE `Users` SET `Ticket` = @ticketKey WHERE `Username` = @user",
                dbconn))
            {
                var ticketKey = RandomProvider.Get().NextUInt32();

                mc.Parameters.AddWithValue("@user", username);
                mc.Parameters.AddWithValue("@ticketKey", ticketKey);

                if(mc.ExecuteNonQuery() == 1)
                    return ticketKey;
            }

            return 0;
        }

        /// <summary>
        /// Retrieves a user from it's session ticket.
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="username"></param>
        /// <param name="ticketKey"></param>
        /// <returns>A new User class if the session was found, null if no session was found</returns>
        public static User RetrieveFromSession(MySqlConnection dbconn, string username, uint ticketKey)
        {
            using (var mc = new MySqlCommand("SELECT * FROM `Users` WHERE `Username` = @user AND `Ticket` = @ticketKey",
                dbconn))
            {
                mc.Parameters.AddWithValue("@user", username);
                mc.Parameters.AddWithValue("@ticketKey", ticketKey);
                
                using (DbDataReader reader = mc.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    
                    return GetUser(reader);
                }
            }
        }
        
        /// <summary>
        /// TODO: Shouldn't we move this to the user class?
        /// </summary>
        /// <param name="dbconn"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static List<Character> RetrieveCharacters(MySqlConnection dbconn, ulong uid)
        {
            /*var command = new MySqlCommand(
                "SELECT Characters.*, vehicles.carType, vehicles.baseColor, teams.TEAMNAME, teams.TMARKID, teams.TEAMRANKING, teams.CLOSEDATE FROM Characters LEFT JOIN teams ON characters.TeamId = teams.TID LEFT JOIN vehicles ON characters.CurrentCarID = vehicles.CID WHERE characters.UID = @uid",
                dbconn);*/
            var command = new MySqlCommand(
                "SELECT * FROM Characters WHERE characters.UID = @uid",
                dbconn);

            command.Parameters.AddWithValue("@uid", uid);

            var chars = new List<Character>();

            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var character = CharacterModel.GetCharacter(dbconn, reader);
                    chars.Add(character);
                }
            }

            foreach (var character in chars)
            {
                character.GarageVehicles = VehicleModel.Retrieve(dbconn, character.Id);
                character.ActiveCar =
                    character.GarageVehicles.Find(vehicle => vehicle.CarID == character.ActiveVehicleId);
                character.Team = TeamModel.Retrieve(dbconn, character.TeamId);
            }

            return chars;
        }

        public static bool SetActiveCharacter(MySqlConnection dbconn, User user, ulong charId)
        {
            using (var mc = new MySqlCommand("UPDATE `Users` SET `LastActiveChar` = @charId WHERE `UID` = @userId",
                dbconn))
            {
                mc.Parameters.AddWithValue("@userId", user.Id);
                mc.Parameters.AddWithValue("@charId", charId);

                return mc.ExecuteNonQuery() == 1;
            }
        }
        
        public static bool Update(MySqlConnection dbconn, User user)
        {
            using (var cmd = new UpdateCommand("UPDATE `Users` SET {0} WHERE `UID` = @userId", dbconn))
            {
                cmd.AddParameter("@userId", user.Id);
                
                //var updateCommand = cmd;
                return Write(user, cmd) == 1;
            }
        }

        public static bool SetSessionTicket(MySqlConnection dbconn, User user)
        {
            using (var mc = new MySqlCommand("UPDATE `Users` SET `Ticket` = @ticketKey WHERE `UID` = @userId",
                dbconn))
            {
                mc.Parameters.AddWithValue("@userId", user.Id);
                mc.Parameters.AddWithValue("@ticketKey", user.Ticket);

                return mc.ExecuteNonQuery() == 1;
            }
        }

        public static User RetrieveFromSerial(MySqlConnection dbconn, ulong userid, ushort vehicleSerial)
        {
            using (var mc = new MySqlCommand("SELECT * FROM `Users` WHERE `UID` = @userId AND `VehicleSerial` = @serial",
                dbconn))
            {
                mc.Parameters.AddWithValue("@userId", userid);
                mc.Parameters.AddWithValue("@serial", vehicleSerial);
                
                using (DbDataReader reader = mc.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    
                    return GetUser(reader);
                }
            }
        }
    }
}