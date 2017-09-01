using System;
using System.Collections.Generic;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Network.GameServer;

namespace Shared.Models
{
    public class FriendModel
    {
        public static List<Friend> Retrieve(MySqlConnection dbconn, ulong characterId)
        {
            var command = new MySqlCommand(
                "SELECT f.*, c.Name, c.channelId, c.City, c.TeamId, c.Level, t.UTEAMNAME, t.TMARKID FROM `friends` as f INNER JOIN characters as c ON c.CID = f.FCID INNER JOIN teams as t ON t.TID = c.TeamId WHERE f.CID=@cid",
                dbconn);
            command.Parameters.AddWithValue("@cid", characterId);

            var friends = new List<Friend>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                    friends.Add(new Friend
                    {
                        ChannelId = (char) Convert.ToInt32(reader["channelId"]), //??
                        CharacterId = Convert.ToInt64(reader["FCID"]),
                        CharacterName = reader["Name"] as string,
                        Level = Convert.ToUInt16(reader["Level"]),
                        State = Convert.ToChar(reader["FSTATE"]),
                        TeamId = Convert.ToInt64(reader["TeamId"]),
                        TeamName = reader["UTEAMNAME"] as string,
                        TeamMarkId = Convert.ToInt64(reader["TMARKID"]),
                        LocationId = Convert.ToUInt16(reader["City"])
                    });
                //SERVERID = Convert.ToInt32(reader["SERVERID"]),
                //FCID = Convert.ToInt32(reader["FCID"]),
                //FSTATE = (char)reader["FSTATE"]
            }
            return friends;
        }

        public static bool AddByName(MySqlConnection dbconn, ulong userActiveCharacterId, string charName,
            char state = 'F')
        {
            // TODO: Check for duplicates
            var command = new MySqlCommand("SELECT CID FROM Characters WHERE Name=@cName", dbconn);
            command.Parameters.AddWithValue("@cName", charName);

            ulong friendId;
            using (DbDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows || !reader.Read())
                    return false;
                // TODO: Add serverid
                friendId = Convert.ToUInt64(reader["CID"]);
            }

            using (var cmd = new InsertCommand("INSERT INTO `friends` {0}", dbconn))
            {
                cmd.Set("CID", userActiveCharacterId);
                cmd.Set("FCID", friendId);
                cmd.Set("FSTATE", state);
                cmd.Execute();
            }

            return true;
        }

        public static void Delete(MySqlConnection dbconn, ulong userActiveCharacterId, string charName)
        {
            ulong friendId;
            using (var command = new MySqlCommand("SELECT CID FROM Characters WHERE Name=@cName", dbconn))
            {
                command.Parameters.AddWithValue("@cName", charName);

                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows || !reader.Read())
                        return;
                    friendId = Convert.ToUInt64(reader["CID"]);
                }
            }

            using (
                var command = new MySqlCommand("DELETE FROM `friends` WHERE CID = @cid AND FCID = @fcid",
                    dbconn))
            {
                command.Parameters.AddWithValue("@cid", userActiveCharacterId);
                command.Parameters.AddWithValue("@fcid", friendId);
                command.ExecuteNonQuery();
            }
        }
    }
}