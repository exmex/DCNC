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

            Log.Info("Username: {0}, PasswordHash: {1}", username, passwordHash);

            User user = null;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User
                    {
                        UID = Convert.ToUInt64(reader["UID"]),
                        Name = reader["Username"] as string,
                        Password = reader["Password"] as string,
                        Salt = reader["Salt"] as string,
                        Permission = Convert.ToInt32(reader["Permission"]),
                        Ticket = Convert.ToUInt32(reader["Ticket"]),
                        Status = (UserStatus) Convert.ToByte(reader["Status"]),
                        CreateIp = reader["CreateIP"] as string
                    };
                }
            }

            return user;
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

            User user = null;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User
                    {
                        UID = Convert.ToUInt64(reader["UID"]),
                        Name = reader["Username"] as string,
                        Password = reader["Password"] as string,
                        Salt = reader["Salt"] as string,
                        Permission = Convert.ToInt32(reader["Permission"]),
                        Ticket = Convert.ToUInt32(reader["Ticket"]),
                        Status = (UserStatus) Convert.ToByte(reader["Status"]),
                        CreateIp = reader["CreateIP"] as string
                    };
                }
            }

            return user;
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

            User user = null;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                    user = new User
                    {
                        UID = Convert.ToUInt64(reader["UID"]),
                        Name = reader["Username"] as string,
                        Password = reader["Password"] as string,
                        Salt = reader["Salt"] as string,
                        Permission = Convert.ToInt32(reader["Permission"]),
                        Ticket = Convert.ToUInt32(reader["Ticket"]),
                        Status = (UserStatus) Convert.ToByte(reader["Status"]),
                        CreateIp = reader["CreateIP"] as string
                    };
            }

            return user;
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

                mc.ExecuteNonQuery();

                return ticketKey;
            }
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

                User user = null;
                using (DbDataReader reader = mc.ExecuteReader())
                {
                    if (reader.Read())
                        user = new User
                        {
                            UID = Convert.ToUInt64(reader["UID"]),
                            Name = reader["Username"] as string,
                            Password = reader["Password"] as string,
                            Salt = reader["Salt"] as string,
                            Permission = Convert.ToInt32(reader["Permission"]),
                            Ticket = Convert.ToUInt32(reader["Ticket"]),
                            Status = (UserStatus) Convert.ToByte(reader["Status"]),
                            CreateIp = reader["CreateIP"] as string
                        };
                }

                return user;
            }
        }
    }
}