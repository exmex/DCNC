using System;
using System.Collections.Generic;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Objects;

namespace Shared.Models
{
    public class CharacterModel
    {
        public static Character Retrieve(MySqlConnection dbconn, string characterName)
        {
            MySqlCommand command = new MySqlCommand(
                "SELECT * FROM Characters WHERE Name = @char", dbconn);

            command.Parameters.AddWithValue("@char", characterName);

            Character character = null;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    character = new Character
                    {
                        Cid = Convert.ToUInt64(reader["CID"]),
                        Uid = Convert.ToUInt64(reader["UID"]),
                        Name = reader["Name"] as string,
                        CreationDate = Convert.ToInt32(reader["CreationDate"]),
                        MitoMoney = Convert.ToInt64(reader["Mito"]),
                        Avatar = Convert.ToUInt16(reader["Avatar"]),
                        Level = Convert.ToUInt16(reader["Level"]),
                        City = Convert.ToInt32(reader["City"]),
                        CurrentCarId = Convert.ToInt32(reader["CurrentCarID"]),
                        InventoryLevel = Convert.ToInt32(reader["InventoryLevel"]),
                        GarageLevel = Convert.ToInt32(reader["GarageLevel"]),
                        Tid = Convert.ToInt64(reader["TID"]),
                        PositionX = (float)Convert.ToDouble(reader["posX"]),
                        PositionY = (float)Convert.ToDouble(reader["posY"]),
                        PositionZ = (float)Convert.ToDouble(reader["posZ"]),
                        Rotation = (float)Convert.ToDouble(reader["posW"]),
                        posState = Convert.ToInt32(reader["posState"])
                    };
                }
            }

            return character;
        }

        public static Character RetrieveOne(MySqlConnection dbconn, ulong cid)
        {
            MySqlCommand command = new MySqlCommand(
                "SELECT * FROM Characters WHERE CID = @cid", dbconn);

            command.Parameters.AddWithValue("@cid", cid);

            Character character = null;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    character = new Character
                    {
                        Cid = Convert.ToUInt64(reader["CID"]),
                        Uid = Convert.ToUInt64(reader["UID"]),
                        Name = reader["Name"] as string,
                        TeamName = "Staff",
                        CreationDate = Convert.ToInt32(reader["CreationDate"]),
                        MitoMoney = Convert.ToInt64(reader["Mito"]),
                        Avatar = Convert.ToUInt16(reader["Avatar"]),
                        Level = Convert.ToUInt16(reader["Level"]),
                        City = Convert.ToInt32(reader["City"]),
                        CurrentCarId = Convert.ToInt32(reader["CurrentCarID"]),
                        InventoryLevel = Convert.ToInt32(reader["InventoryLevel"]),
                        GarageLevel = Convert.ToInt32(reader["GarageLevel"]),
                        Tid = Convert.ToInt64(reader["TID"]),
                        PositionX = (float)Convert.ToDouble(reader["posX"]),
                        PositionY = (float)Convert.ToDouble(reader["posY"]),
                        PositionZ = (float)Convert.ToDouble(reader["posZ"]),
                        Rotation = (float)Convert.ToDouble(reader["posW"]),
                        posState = Convert.ToInt32(reader["posState"])
                    };
                }
            }

            return character;
        }

        public static List<Character> Retrieve(MySqlConnection dbconn, ulong uid)
        {
            MySqlCommand command = new MySqlCommand(
                "SELECT * FROM Characters WHERE UID = @uid", dbconn);

            command.Parameters.AddWithValue("@uid", uid);

            List<Character> chars = new List<Character>();

            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var character = new Character
                    {
                        Cid = Convert.ToUInt64(reader["CID"]),
                        Uid = Convert.ToUInt64(reader["UID"]),
                        Name = reader["Name"] as string,
                        TeamName = "Staff",
                        CreationDate = Convert.ToInt32(reader["CreationDate"]),
                        MitoMoney = Convert.ToInt64(reader["Mito"]),
                        Avatar = Convert.ToUInt16(reader["Avatar"]),
                        Level = Convert.ToUInt16(reader["Level"]),
                        City = Convert.ToInt32(reader["City"]),
                        CurrentCarId = Convert.ToInt32(reader["CurrentCarID"]),
                        InventoryLevel = Convert.ToInt32(reader["InventoryLevel"]),
                        GarageLevel = Convert.ToInt32(reader["GarageLevel"]),
                        Tid = Convert.ToInt64(reader["TID"]),
                        PositionX = (float)Convert.ToDouble(reader["posX"]),
                        PositionY = (float)Convert.ToDouble(reader["posY"]),
                        PositionZ = (float)Convert.ToDouble(reader["posZ"]),
                        Rotation = (float)Convert.ToDouble(reader["posW"]),
                        posState = Convert.ToInt32(reader["posState"]),
                    };
                    chars.Add(character);
                }
            }

            return chars;
        }

        public static void UpdatePosition(MySqlConnection dbconn, ulong charId,
               int channelId, float x, float y, float z, float w, int cityId, int posState)
        {
            using (var cmd = new UpdateCommand("UPDATE characters SET {0} WHERE CID=@charId", dbconn))
            {
                cmd.AddParameter("@charId", charId);
                cmd.Set("posX", x);
                cmd.Set("posY", y);
                cmd.Set("posZ", z);
                cmd.Set("posW", w);
                cmd.Set("City", cityId);
                cmd.Set("channelId", channelId);
                cmd.Set("posState", posState);
                cmd.Execute();
            }
        }

        // Maybe rename this to OwnsCharacter?
        public static bool HasCharacter(MySqlConnection dbconn, ulong cid, ulong uid)
        {
            MySqlCommand command = new MySqlCommand(
                "SELECT * FROM Characters WHERE CID = @cid AND UID = @uid", dbconn);

            command.Parameters.AddWithValue("@cid", cid);
            command.Parameters.AddWithValue("@uid", uid);

            using (DbDataReader reader = command.ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        public static void DeleteCharacter(MySqlConnection dbconn, ulong cid, ulong uid)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM Characters WHERE CID = @cid AND UID = @uid", dbconn);
            command.Parameters.AddWithValue("@cid", cid);
            command.Parameters.AddWithValue("@uid", uid);
            command.ExecuteNonQuery();
        }

        public static bool Exists(MySqlConnection dbconn, string characterName)
        {
            MySqlCommand command = new MySqlCommand(
                "SELECT * FROM Characters WHERE Name = @charName", dbconn);

            command.Parameters.AddWithValue("@charName", characterName);

            using (DbDataReader reader = command.ExecuteReader())
            {
                return reader.HasRows;
            }
        }
		
		public static void CreateCharacter(MySqlConnection dbconn, ulong uid, string characterName, short avatar, uint carType, uint carColor)
		{
			long insertedCharId = -1;
			long insertedCarId = -1;
			
			using (var cmd = new InsertCommand("INSERT INTO `Characters` {0}", dbconn))
            {
				cmd.Set("UID", uid);
                cmd.Set("Name", characterName);
                cmd.Set("Avatar", avatar);
                cmd.Set("CurrentCarId", -1); // Invalidate this.
                cmd.Set("City", 1);
				
                cmd.Execute();
				insertedCharId = cmd.LastId;
			}
			using (var cmd = new InsertCommand("INSERT INTO `vehicles` {0}", dbconn))
			{
				cmd.Set("CharID", insertedCharId);
				cmd.Set("color", carColor);
				cmd.Set("carType", carType);
				
				cmd.Execute();
				insertedCarId = cmd.LastId;
			}
			using (var cmd = new UpdateCommand("UPDATE `Characters` SET {0} WHERE `CID` = @charId", dbconn))
			{
				cmd.AddParameter("@charId", insertedCharId);
				cmd.Set("CurrentCarID", insertedCarId);
				
				cmd.Execute();
			}
		}
    }
}