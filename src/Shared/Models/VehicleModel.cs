using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Objects;

namespace Shared.Models
{
    public class VehicleModel
    {
        public static Vehicle Retrieve(MySqlConnection dbconn, string charname)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM Characters WHERE Name = @char", dbconn);

            command.Parameters.AddWithValue("@char", charname);

            Vehicle vehicle = null;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    vehicle = new Vehicle();
                    vehicle.CarID = Convert.ToUInt32(reader["CID"]);
                    vehicle.AuctionCnt = Convert.ToUInt32(reader["auctionCount"]);
                    vehicle.BaseColor = Convert.ToUInt32(reader["baseColor"]);

                    vehicle.CarType = Convert.ToUInt32(reader["carType"]);
                    vehicle.Grade = Convert.ToUInt32(reader["grade"]);
                    vehicle.Mitron = (float) Convert.ToDouble(reader["mitron"]);
                    vehicle.Kmh = (float) Convert.ToDouble(reader["kmh"]);
                    vehicle.SlotType = Convert.ToUInt32(reader["slotType"]);
                    vehicle.Color = Convert.ToUInt32(reader["color"]);
                    vehicle.MitronCapacity = (float) Convert.ToDouble(reader["mitronCapacity"]);
                    vehicle.MitronEfficiency = (float) Convert.ToDouble(reader["mitronEfficiency"]);
                }
            }

            return vehicle;
        }

        public static Vehicle Retrieve(MySqlConnection dbconn, int carIdx)
        {
            MySqlCommand command = new MySqlCommand(
                "SELECT * FROM Vehicles WHERE CID = @car", dbconn);

            command.Parameters.AddWithValue("@car", carIdx);

            Vehicle vehicle = null;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    vehicle = new Vehicle();
                    vehicle.CarID = Convert.ToUInt32(reader["CID"]);
                    vehicle.AuctionCnt = Convert.ToUInt32(reader["auctionCount"]);
                    vehicle.BaseColor = Convert.ToUInt32(reader["baseColor"]);

                    vehicle.CarType = Convert.ToUInt32(reader["carType"]);
                    vehicle.Grade = Convert.ToUInt32(reader["grade"]);
                    vehicle.Mitron = (float) Convert.ToDouble(reader["mitron"]);
                    vehicle.Kmh = (float) Convert.ToDouble(reader["kmh"]);
                    vehicle.SlotType = Convert.ToUInt32(reader["slotType"]);
                    vehicle.Color = Convert.ToUInt32(reader["color"]);
                    vehicle.MitronCapacity = (float) Convert.ToDouble(reader["mitronCapacity"]);
                    vehicle.MitronEfficiency = (float) Convert.ToDouble(reader["mitronEfficiency"]);
                }
            }

            return vehicle;
        }

        public static List<Vehicle> Retrieve(MySqlConnection dbconn, ulong cid)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM Vehicles WHERE CharID = @cid", dbconn);

            command.Parameters.AddWithValue("@cid", cid);

            List<Vehicle> vehicles = new List<Vehicle>();

            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var vehicle = new Vehicle();
                    vehicle = new Vehicle();
                    vehicle.CarID = Convert.ToUInt32(reader["CID"]);
                    vehicle.AuctionCnt = Convert.ToUInt32(reader["auctionCount"]);
                    vehicle.BaseColor = Convert.ToUInt32(reader["baseColor"]);

                    vehicle.CarType = Convert.ToUInt32(reader["carType"]);
                    vehicle.Grade = Convert.ToUInt32(reader["grade"]);
                    vehicle.Mitron = (float) Convert.ToDouble(reader["mitron"]);
                    vehicle.Kmh = (float) Convert.ToDouble(reader["kmh"]);
                    vehicle.SlotType = Convert.ToUInt32(reader["slotType"]);
                    vehicle.Color = Convert.ToUInt32(reader["color"]);
                    vehicle.MitronCapacity = (float) Convert.ToDouble(reader["mitronCapacity"]);
                    vehicle.MitronEfficiency = (float) Convert.ToDouble(reader["mitronEfficiency"]);
                    vehicles.Add(vehicle);
                }
            }

            return vehicles;
        }
    }
}
