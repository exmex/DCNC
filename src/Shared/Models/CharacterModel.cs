using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Numerics;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Objects;
using Shared.Util;

namespace Shared.Models
{
    /// <summary>
    /// </summary>
    public static class CharacterModel
    {
        public static Character GetCharacter(MySqlConnection dbconn, DbDataReader reader)
        {
            var character = new Character();
            character.Id = Convert.ToUInt64(reader["CID"]);
            character.Uid = Convert.ToUInt64(reader["UID"]);
            character.Name = reader["Name"] as string;
            character.CreationDate = Convert.ToInt32(reader["CreationDate"]);
            character.MitoMoney = Convert.ToInt64(reader["Mito"]);
            character.Hancoin = Convert.ToInt32(reader["Hancoin"]);
            character.Avatar = Convert.ToUInt16(reader["Avatar"]);
            character.Guild = Convert.ToInt16(reader["Guild"]);
            character.Level = Convert.ToUInt16(reader["Level"]);
            
            character.ExperienceInfo.BaseExp = Convert.ToInt32(reader["BaseExp"]);
            character.ExperienceInfo.CurExp = Convert.ToInt32(reader["CurExp"]);
            character.ExperienceInfo.NextExp = Convert.ToInt32(reader["NextExp"]);
            character.TotalDistance = Convert.ToInt32(reader["Mileage"]);
            character.City = Convert.ToInt32(reader["City"]);
            character.ActiveVehicleId = Convert.ToUInt32(reader["CurrentCarID"]);
            character.InventoryLevel = Convert.ToInt32(reader["InventoryLevel"]);
            character.GarageLevel = Convert.ToInt32(reader["GarageLevel"]);
            character.TeamId = Convert.ToInt64(reader["TeamId"]);
            character.TeamRank = Convert.ToInt32(reader["TeamRank"]);
            character.Position = new Vector4(Convert.ToSingle(reader["posX"]), Convert.ToSingle(reader["posY"]), Convert.ToSingle(reader["posZ"]), Convert.ToSingle(reader["posW"]));
            character.LastChannel = Convert.ToInt32(reader["channelId"]);
            character.PosState = Convert.ToInt32(reader["posState"]);
            return character;
        }

        public static int WriteCharacter(Character character, UpdateCommand cmd)
        {
            //cmd.Set("CID", Cid);
            //cmd.Set("UID", Uid);
            cmd.Set("Name", character.Name);
            //cmd.Set("TEAMNAME", character. TeamName );
            cmd.Set("CreationDate", character.CreationDate);
            cmd.Set("Mito", character.MitoMoney);
            cmd.Set("Hancoin", character.Hancoin);
            cmd.Set("Avatar", character.Avatar);
            cmd.Set("Level", character.Level);
            cmd.Set("BaseExp", character.ExperienceInfo.BaseExp);
            cmd.Set("CurExp", character.ExperienceInfo.CurExp);
            cmd.Set("NextExp", character.ExperienceInfo.NextExp);
            cmd.Set("Mileage", character.TotalDistance);
            cmd.Set("City", character.City);
            cmd.Set("CurrentCarID", character.ActiveVehicleId);
            cmd.Set("InventoryLevel", character.InventoryLevel);
            cmd.Set("GarageLevel", character.GarageLevel);
            cmd.Set("TeamId", character.TeamId);
            cmd.Set("TeamRank", character.TeamRank);
            cmd.Set("posX", character.Position.X);
            cmd.Set("posY", character.Position.Y);
            cmd.Set("posZ", character.Position.Z);
            cmd.Set("posW", character.Position.W);
            cmd.Set("channelId", character.LastChannel);
            cmd.Set("posState", character.PosState);

            return cmd.Execute();
        }
        
        public static Character Retrieve(MySqlConnection dbconn, string characterName)
        {
            var command = new MySqlCommand(
                "SELECT * FROM Characters WHERE characters.Name = @char", dbconn);

            command.Parameters.AddWithValue("@char", characterName);

            Character character;
            using (DbDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read()) return null;
                character = GetCharacter(dbconn, reader);
            }
            character.GarageVehicles = VehicleModel.Retrieve(dbconn, character.Id);
            character.ActiveCar = character.GarageVehicles.Find(vehicle => vehicle.CarId == character.ActiveVehicleId);
            character.Team = TeamModel.Retrieve(dbconn, character.TeamId);
            return character;
        }

        public static Character Retrieve(MySqlConnection dbconn, ulong cid)
        {
            var command = new MySqlCommand(
                "SELECT * FROM Characters WHERE characters.CID = @cid", dbconn);

            command.Parameters.AddWithValue("@cid", cid);

            Character character;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read()) return null;
                character = GetCharacter(dbconn, reader);
            }
            character.GarageVehicles = VehicleModel.Retrieve(dbconn, character.Id);
            character.ActiveCar = character.GarageVehicles.Find(vehicle => vehicle.CarId == character.ActiveVehicleId);
            character.Team = TeamModel.Retrieve(dbconn, character.TeamId);
            return character;
        }
        
        /// <summary>
        /// Checks wether the userId owns the characterId
        /// TODO: Maybe rename this to OwnsCharacter?
        /// </summary>
        /// <param name="dbconn">The mysql database connection</param>
        /// <param name="cid">The id of the character</param>
        /// <param name="uid">The id of the user</param>
        /// <returns></returns>
        public static bool HasCharacter(MySqlConnection dbconn, ulong cid, ulong uid)
        {
            var command = new MySqlCommand(
                "SELECT * FROM Characters WHERE CID = @cid AND UID = @uid", dbconn);

            command.Parameters.AddWithValue("@cid", cid);
            command.Parameters.AddWithValue("@uid", uid);

            using (DbDataReader reader = command.ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        /// <summary>
        /// Checks wether the userId owns the character name
        /// TODO: Maybe rename this to OwnsCharacter?
        /// </summary>
        /// <param name="dbconn">The mysql database connection</param>
        /// <param name="characterName">the name of the character</param>
        /// <param name="uid">The id of the user</param>
        /// <returns></returns>
        public static ulong HasCharacter(MySqlConnection dbconn, string characterName, ulong uid)
        {
            var command = new MySqlCommand(
                "SELECT `CID` FROM Characters WHERE Name = @charName AND UID = @uid", dbconn);

            command.Parameters.AddWithValue("@charName", characterName);
            command.Parameters.AddWithValue("@uid", uid);

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read()) return 0;
                return reader.HasRows ? Convert.ToUInt64(reader["CID"]) : 0;
            }
        }
        
        /// <summary>
        /// Deletes the character from the DB
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="cid">The id of the character</param>
        /// <param name="uid">The id of the user</param>
        public static void DeleteCharacter(MySqlConnection dbconn, ulong cid, ulong uid)
        {
            var command = new MySqlCommand("DELETE FROM Characters WHERE CID = @cid AND UID = @uid", dbconn);
            command.Parameters.AddWithValue("@cid", cid);
            command.Parameters.AddWithValue("@uid", uid);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks wether a character name is already taken
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="characterName">The character name to check</param>
        /// <returns>true if the name already exists, false otherwise</returns>
        public static bool CheckNameExists(MySqlConnection dbconn, string characterName)
        {
            var command = new MySqlCommand(
                "SELECT * FROM Characters WHERE Name = @charName", dbconn);

            command.Parameters.AddWithValue("@charName", characterName);

            using (DbDataReader reader = command.ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        public static void CreateCharacter(MySqlConnection dbconn, ref Character character
            /*ulong uid, string characterName, short avatar,
            uint carType, uint carColor*/)
        {
            using (var cmd = new InsertCommand("INSERT INTO `Characters` {0}", dbconn))
            {
                cmd.Set("UID", character.Uid);
                cmd.Set("Name", character.Name);
                cmd.Set("Avatar", character.Avatar);
                // The CarId will be updated once we have inserted the car into the db
                cmd.Set("CurrentCarId", -1);
                cmd.Set("City", character.City);
                cmd.Set("CreationDate", DateTimeOffset.Now.ToUnixTimeSeconds());
                cmd.Set("Level", character.Level);
                cmd.Set("GarageLevel", character.GarageLevel);
                cmd.Set("InventoryLevel", character.InventoryLevel);
                cmd.Set("posState", character.PosState);
                cmd.Set("channelId", character.LastChannel);
                cmd.Set("Mito", character.MitoMoney);
                cmd.Set("Hancoin", character.Hancoin);

                cmd.Execute();
                character.Id = (ulong)cmd.LastId;
            }
        }

        public static bool Update(MySqlConnection dbconn, Character character)
        {
            using (var cmd = new UpdateCommand("UPDATE `Characters` SET {0} WHERE `CID` = @charId", dbconn))
            {
                cmd.AddParameter("@charId", character.Id);
                
                //var updateCommand = cmd;
                return WriteCharacter(character, cmd) == 1;
                //character.WriteToDb(ref updateCommand);
                //cmd.Execute();
            }
        }

        /*public static Character Retrieve(MySqlConnection dbconn, string characterName)
        {
            var command = new MySqlCommand(
                "SELECT * FROM Characters WHERE Name = @char", dbconn);

            command.Parameters.AddWithValue("@char", characterName);

            Character character = null;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                    character = new Character
                    {
                        Cid = Convert.ToUInt64(reader["CID"]),
                        Uid = Convert.ToUInt64(reader["UID"]),
                        Name = reader["Name"] as string,
                        CreationDate = Convert.ToInt32(reader["CreationDate"]),
                        MitoMoney = Convert.ToInt64(reader["Mito"]),
                        Avatar = Convert.ToUInt16(reader["Avatar"]),
                        Level = Convert.ToUInt16(reader["Level"]),
                        BaseExp = Convert.ToInt32(reader["BaseExp"]),
                        CurExp = Convert.ToInt32(reader["CurExp"]),
                        NextExp = Convert.ToInt32(reader["NextExp"]),
                        City = Convert.ToInt32(reader["City"]),
                        CurrentCarId = Convert.ToInt32(reader["CurrentCarID"]),
                        InventoryLevel = Convert.ToInt32(reader["InventoryLevel"]),
                        GarageLevel = Convert.ToInt32(reader["GarageLevel"]),
                        Tid = Convert.ToInt64(reader["TID"]),
                        PositionX = (float) Convert.ToDouble(reader["posX"]),
                        PositionY = (float) Convert.ToDouble(reader["posY"]),
                        PositionZ = (float) Convert.ToDouble(reader["posZ"]),
                        Rotation = (float) Convert.ToDouble(reader["posW"]),
                        posState = Convert.ToInt32(reader["posState"])
                    };
            }

            return character;
        }

        public static Character RetrieveOne(MySqlConnection dbconn, ulong cid)
        {
            var command = new MySqlCommand(
                "SELECT * FROM Characters WHERE CID = @cid", dbconn);

            command.Parameters.AddWithValue("@cid", cid);

            Character character = null;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
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
                        BaseExp = Convert.ToInt32(reader["BaseExp"]),
                        CurExp = Convert.ToInt32(reader["CurExp"]),
                        NextExp = Convert.ToInt32(reader["NextExp"]),
                        City = Convert.ToInt32(reader["City"]),
                        CurrentCarId = Convert.ToInt32(reader["CurrentCarID"]),
                        HancoinInven = Convert.ToInt32(reader["InventoryLevel"]),
                        HancoinGarage = Convert.ToInt32(reader["GarageLevel"]),
                        Tid = Convert.ToInt64(reader["TID"]),
                        PositionX = (float) Convert.ToDouble(reader["posX"]),
                        PositionY = (float) Convert.ToDouble(reader["posY"]),
                        PositionZ = (float) Convert.ToDouble(reader["posZ"]),
                        Rotation = (float) Convert.ToDouble(reader["posW"]),
                        posState = Convert.ToInt32(reader["posState"])
                    };
            }

            return character;
        }

        public static List<Character> Retrieve(MySqlConnection dbconn, ulong uid)
        {
            var command = new MySqlCommand(
                "SELECT characters.*, vehicles.carType, vehicles.baseColor FROM characters INNER JOIN vehicles ON characters.CurrentCarID = vehicles.CID WHERE characters.UID = @uid",
                dbconn);

            command.Parameters.AddWithValue("@uid", uid);

            var chars = new List<Character>();

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
                        BaseExp = Convert.ToInt32(reader["BaseExp"]),
                        CurExp = Convert.ToInt32(reader["CurExp"]),
                        NextExp = Convert.ToInt32(reader["NextExp"]),
                        City = Convert.ToInt32(reader["City"]),
                        CurrentCarId = Convert.ToInt32(reader["CurrentCarID"]),
                        HancoinInven = Convert.ToInt32(reader["InventoryLevel"]),
                        HancoinGarage = Convert.ToInt32(reader["GarageLevel"]),
                        Tid = Convert.ToInt64(reader["TID"]),
                        PositionX = (float) Convert.ToDouble(reader["posX"]),
                        PositionY = (float) Convert.ToDouble(reader["posY"]),
                        PositionZ = (float) Convert.ToDouble(reader["posZ"]),
                        Rotation = (float) Convert.ToDouble(reader["posW"]),
                        posState = Convert.ToInt32(reader["posState"]),
                        ActiveCar = new Vehicle
                        {
                            BaseColor = Convert.ToUInt32(reader["baseColor"]),
                            CarType = Convert.ToUInt32(reader["carType"])
                        }
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
            var command = new MySqlCommand(
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
            var command = new MySqlCommand("DELETE FROM Characters WHERE CID = @cid AND UID = @uid", dbconn);
            command.Parameters.AddWithValue("@cid", cid);
            command.Parameters.AddWithValue("@uid", uid);
            command.ExecuteNonQuery();
        }

        public static bool Exists(MySqlConnection dbconn, string characterName)
        {
            var command = new MySqlCommand(
                "SELECT * FROM Characters WHERE Name = @charName", dbconn);

            command.Parameters.AddWithValue("@charName", characterName);

            using (DbDataReader reader = command.ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        public static void CreateCharacter(MySqlConnection dbconn, ulong uid, string characterName, short avatar,
            uint carType, uint carColor)
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
                cmd.Set("CreationDate", DateTimeOffset.Now.ToUnixTimeSeconds());
                cmd.Set("Level", 1);
                cmd.Set("GarageLevel", 1);
                cmd.Set("InventoryLevel", 1);
                cmd.Set("posState", 1);
                cmd.Set("channelId", -1);
                // TODO: Change to packet Cmd_FirstPositon.
                cmd.Set("posX", -2157.2f + 4 * (new Random().Next() % 10));
                cmd.Set("posY", -205.05 + 4 * (new Random().Next() % 10));
                cmd.Set("posZ", 85.720001 + 4 * (new Random().Next() % 10));
                cmd.Set("posW", 90.967003 + 4 * (new Random().Next() % 10));

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

        public static void UpdateExp(MySqlConnection dbconn, Character character)
        {
            using (var cmd = new UpdateCommand("UPDATE characters SET {0} WHERE CID=@charId", dbconn))
            {
                cmd.AddParameter("@charId", character.Cid);
                cmd.Set("BaseExp", character.BaseExp);
                cmd.Set("CurExp", character.CurExp);
                cmd.Set("NextExp", character.NextExp);
                cmd.Execute();
            }
        }

        public static bool UpdateExp(MySqlConnection dbconn, string characterName, int amount, bool set = true)
        {
            if (!Exists(dbconn, characterName)) return false;

            long currentExp = 0;
            if (!set)
            {
                var command = new MySqlCommand(
                    "SELECT * FROM Characters WHERE `Name` = @char", dbconn);

                command.Parameters.AddWithValue("@char", characterName);
                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        currentExp = Convert.ToInt64(reader["CurExp"]);
                }
            }
            
            using (var cmd = new UpdateCommand("UPDATE `Characters` SET {0} WHERE `Name` = @charName", dbconn))
            {
                cmd.AddParameter("@charName", characterName);
                if(set)
                    cmd.Set("CurExp", amount);
                else
                    cmd.Set("CurExp", currentExp+amount);

                if (cmd.Execute() == 1)
                    return true;
            }

            return false;
        }
        
        public static void UpdateMito(MySqlConnection dbconn, Character character)
        {
            using (var cmd = new UpdateCommand("UPDATE characters SET {0} WHERE CID=@charId", dbconn))
            {
                cmd.AddParameter("@charId", character.Cid);
                cmd.Set("Mito", character.MitoMoney);
                cmd.Execute();
            }
        }
        
        public static bool UpdateMito(MySqlConnection dbconn, string characterName, long amount, bool set = true)
        {
            if (!Exists(dbconn, characterName)) return false;

            long currentMoney = 0;
            if (!set)
            {
                var command = new MySqlCommand(
                    "SELECT * FROM Characters WHERE `Name` = @char", dbconn);

                command.Parameters.AddWithValue("@char", characterName);
                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        currentMoney = Convert.ToInt64(reader["Mito"]);
                }
            }
            
            using (var cmd = new UpdateCommand("UPDATE `Characters` SET {0} WHERE `Name` = @charName", dbconn))
            {
                cmd.AddParameter("@charName", characterName);
                if(set)
                    cmd.Set("Mito", amount);
                else
                    cmd.Set("Mito", currentMoney+amount);

                if (cmd.Execute() == 1)
                    return true;
            }

            return false;
        }*/
    }
}