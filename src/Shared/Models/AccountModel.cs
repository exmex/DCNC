using System;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Objects;
using Shared.Util;

namespace Shared.Models
{
    public class AccountModel
    {
        private const int _saltSize = 32;

        public static User Retrieve(MySqlConnection dbconn, string username, string passwordHash)
        {
            MySqlCommand command = new MySqlCommand(
                "SELECT * FROM Users WHERE Username = @user AND Password = @pwhash", dbconn);

            command.Parameters.AddWithValue("@user", username);
            command.Parameters.AddWithValue("@pwhash", passwordHash);

            Log.Info("Username: {0}, PasswordHash: {1}", username, passwordHash);

            User user = null;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User();
                    user.UID = Convert.ToUInt64(reader["UID"]);
                    user.Name = reader["Username"] as string;
                    user.Password = reader["Password"] as string;
                    user.Salt = reader["Salt"] as string;
                    user.Ticket = Convert.ToUInt32(reader["Ticket"]);
                    user.Status = (UserStatus)Convert.ToByte(reader["Status"]);
                    user.CreateIp = reader["CreateIP"] as string;
                }
            }

            return user;
        }

        public static User Retrieve(MySqlConnection dbconn, string username)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM Users WHERE Username = @user", dbconn);

            command.Parameters.AddWithValue("@user", username);

            User user = null;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User();
                    user.UID = Convert.ToUInt64(reader["UID"]);
                    user.Name = reader["Username"] as string;
                    user.Password = reader["Password"] as string;
                    user.Salt = reader["Salt"] as string;
                    user.Ticket = Convert.ToUInt32(reader["Ticket"]);
                    user.Status = (UserStatus)Convert.ToByte(reader["Status"]);
                    user.CreateIp = reader["CreateIP"] as string;
                }
            }

            return user;
        }

        public static User Retrieve(MySqlConnection dbconn, ulong uid)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM Users WHERE UID = @uid", dbconn);

            command.Parameters.AddWithValue("@uid", uid);

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
                        Ticket = Convert.ToUInt32(reader["Ticket"]),
                        Status = (UserStatus) Convert.ToByte(reader["Status"]),
                        CreateIp = reader["CreateIP"] as string
                    };
                }
            }

            return user;
        }

        /// <summary>
        /// Adds new account to the database.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="accountId"></param>
        /// <param name="password"></param>
        public static void CreateAccount(MySqlConnection dbconn, string ip, string accountId, string password)
        {
            var salt = Password.CreateSalt(_saltSize);
            password = Password.GenerateSaltedHash(password, salt);
            
            using (var cmd = new InsertCommand("INSERT INTO `Users` {0}", dbconn))
            {
                cmd.Set("Username", accountId);
                cmd.Set("Password", password);
                cmd.Set("Salt", salt);
                cmd.Set("Status", 1);
                cmd.Set("CreateIP", ip);
                cmd.Set("CreateDate", DateTime.Now);
                cmd.Set("Ticket", 0);

                cmd.Execute();
            }
        }

        public static bool AccountExists(MySqlConnection dbconn, string accountName)
        {
            var mc = new MySqlCommand("SELECT `UID` FROM `Users` WHERE `Username` = @user", dbconn);
            mc.Parameters.AddWithValue("@user", accountName);

            using (var reader = mc.ExecuteReader())
                return reader.HasRows;
        }

        public static void SetAccountPassword(MySqlConnection dbconn, string accountName, string password)
        {
            using (var mc = new MySqlCommand("UPDATE `Users` SET `Password` = @password, `Salt` = @salt WHERE `Username` = @user", dbconn))
            {
                var salt = Password.CreateSalt(_saltSize);
                mc.Parameters.AddWithValue("@user", accountName);
                mc.Parameters.AddWithValue("@password", Password.GenerateSaltedHash(password, salt));
                mc.Parameters.AddWithValue("@salt", salt);

                mc.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Sets new randomized session key for the account and returns it.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static uint CreateSession(MySqlConnection dbconn, string accountId)
        {
            using (var mc = new MySqlCommand("UPDATE `Users` SET `Ticket` = @ticketKey WHERE `Username` = @user", dbconn))
            {
                var ticketKey = RandomProvider.Get().NextUInt32();

                mc.Parameters.AddWithValue("@user", accountId);
                mc.Parameters.AddWithValue("@ticketKey", ticketKey);

                mc.ExecuteNonQuery();

                return ticketKey;
            }
        }

        /// <summary>
        /// Returns true if sessionKey is correct for account.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ticketKey"></param>
        /// <returns></returns>
        public static User GetSession(MySqlConnection dbconn, string accountId, uint ticketKey)
        {
            using (var mc = new MySqlCommand("SELECT * FROM `Users` WHERE `Username` = @user AND `Ticket` = @ticketKey", dbconn))
            {
                mc.Parameters.AddWithValue("@user", accountId);
                mc.Parameters.AddWithValue("@ticketKey", ticketKey);

                User user = null;
                using (DbDataReader reader = mc.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UID = Convert.ToUInt64(reader["UID"]),
                            Name = reader["Username"] as string,
                            Password = reader["Password"] as string,
                            Salt = reader["Salt"] as string,
                            Ticket = Convert.ToUInt32(reader["Ticket"]),
                            Status = (UserStatus) Convert.ToByte(reader["Status"]),
                            CreateIp = reader["CreateIP"] as string
                        };
                    }
                }

                return user;
            }
        }
    }
}
