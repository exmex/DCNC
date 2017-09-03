using System;
using System.Collections.Generic;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Objects;

namespace Shared.Models
{
    public class VehicleModel
    {
        /// <summary>
        /// Writes the vehicle to the database
        /// </summary>
        /// <param name="dbconn">The mysql connection</param>
        /// <param name="vehicle">The vehicle that should be written</param>
        public static void Update(MySqlConnection dbconn, Vehicle vehicle)
        {
            using (var cmd = new UpdateCommand("UPDATE vehicles SET {0} WHERE CID=@vehId", dbconn))
            {
                cmd.AddParameter("@vehId", vehicle.CarID);
                var updateCommand = cmd;
                vehicle.WriteToDb(ref updateCommand);
            }
        }
        
        /// <summary>
        /// Retrieves a vehicle by id from the database
        /// </summary>
        /// <param name="dbconn">The mysql database connection</param>
        /// <param name="carId">The Id of the vehicle to retrieve</param>
        /// <returns>A new Vehicle class, null if none was found</returns>
        public static Vehicle Retrieve(MySqlConnection dbconn, uint carId)
        {
            var vehicle = new Vehicle();
            
            var command = new MySqlCommand(
                "SELECT * FROM Vehicles WHERE CID = @car", dbconn);

            command.Parameters.AddWithValue("@car", carId);

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read()) return null;
                vehicle.ReadFromDb(reader);
            }

            return vehicle;
        }
        
        /// <summary>
        /// Retrieves a list of vehicles from the specified character id
        /// TODO: Shouldn't we move this to the CharacterModel?
        /// </summary>
        /// <param name="dbconn">The mysql database connection</param>
        /// <param name="cid">The id of the character to retrieve vehicles from</param>
        /// <returns>A List of Vehicle classes</returns>
        public static List<Vehicle> Retrieve(MySqlConnection dbconn, ulong cid)
        {
            var command = new MySqlCommand("SELECT * FROM Vehicles WHERE CharID = @cid", dbconn);

            command.Parameters.AddWithValue("@cid", cid);

            var vehicles = new List<Vehicle>();

            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var vehicle = new Vehicle();
                    vehicle.ReadFromDb(reader);
                    vehicles.Add(vehicle);
                }
            }

            return vehicles;
        }
        
        public static long Create(MySqlConnection dbconn, Vehicle veh, ulong ownerId = 0)
        {
            using (var cmd = new InsertCommand("INSERT INTO `vehicles` {0}", dbconn))
            {
                if(ownerId != 0UL)
                    cmd.Set("CharID", ownerId);
                var insertCommand = cmd;
                veh.WriteToDb(ref insertCommand);
                return cmd.LastId;
            }
        }

        public static int RetrieveCount(MySqlConnection dbconn, ulong charId)
        {
            var command = new MySqlCommand("SELECT COUNT(*) FROM Vehicles WHERE CharID = @cid", dbconn);

            command.Parameters.AddWithValue("@cid", charId);
            using (var reader = command.ExecuteReader())
            {
                return reader.GetInt32(0);
            }
        }
    }
}