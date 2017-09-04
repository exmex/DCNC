using System;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Objects;
using Shared.Util;

namespace Shared.Models
{
    /// <summary>
    /// TODO: Move this to the User class?
    /// </summary>
    public static class AccountModel
    {
        private const int SaltSize = 32;

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
                
                return User.ReadFromDb(reader);
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
                if (!reader.Read()) return null;
                
                return User.ReadFromDb(reader);
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
                return User.ReadFromDb(reader);
            }
        }

        /// <summary>
        ///     Adds new account to the database.
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="ip">The ip of the user</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password in plain-text</param>
        public static void CreateAccount(MySqlConnection dbconn, string ip, string username, string password)
        {
            var salt = Password.CreateSalt(SaltSize);
            password = Password.GenerateSaltedHash(password, salt);

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
            }
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
        
        /// <summary>
        ///     Sets the account password to the specified password
        ///     Handles hashing!
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password in plain-text</param>
        public static void SetAccountPassword(MySqlConnection dbconn, string username, string password)
        {
            using (var mc =
                new MySqlCommand("UPDATE `Users` SET `Password` = @password, `Salt` = @salt WHERE `Username` = @user",
                    dbconn))
            {
                var salt = Password.CreateSalt(SaltSize);
                mc.Parameters.AddWithValue("@user", username);
                mc.Parameters.AddWithValue("@password", Password.GenerateSaltedHash(password, salt));
                mc.Parameters.AddWithValue("@salt", salt);

                mc.ExecuteNonQuery();
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
        ///     Returns true if sessionKey is correct for account.
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="accountId"></param>
        /// <param name="ticketKey"></param>
        /// <returns>A new User class if the session was found, null if no session was found</returns>
        public static User GetSession(MySqlConnection dbconn, string accountId, uint ticketKey)
        {
            using (var mc = new MySqlCommand("SELECT * FROM `Users` WHERE `Username` = @user AND `Ticket` = @ticketKey",
                dbconn))
            {
                mc.Parameters.AddWithValue("@user", accountId);
                mc.Parameters.AddWithValue("@ticketKey", ticketKey);
                
                using (DbDataReader reader = mc.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    
                    return User.ReadFromDb(reader);
                }
            }
        }

        public static bool SetActiveCharacter(MySqlConnection dbconn, User user, ulong charId)
        {
            using (var mc = new MySqlCommand("UPDATE `Users` SET `LastActiveChar` = @charId WHERE `UID` = @userId",
                dbconn))
            {
                mc.Parameters.AddWithValue("@userId", user.UID);
                mc.Parameters.AddWithValue("@charId", charId);

                return mc.ExecuteNonQuery() == 1;
            }
        }
        
        public static bool Update(MySqlConnection dbconn, User user)
        {
            using (var cmd = new UpdateCommand("UPDATE `Users` SET {0} WHERE `UID` = @userId", dbconn))
            {
                cmd.AddParameter("@userId", user.UID);
                
                var updateCommand = cmd;
                user.WriteToDb(ref updateCommand);
                return cmd.Execute() == 1;
            }
        }
    }
}