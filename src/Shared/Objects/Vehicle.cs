using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Util;

namespace Shared.Objects
{
    public class Vehicle : BinaryWriterExt.ISerializable
    {
        public uint AuctionCnt;
        public bool AuctionOn;
        public uint BaseColor;
        public uint CarId;
        public uint CarType;
        public uint Color;

        public uint Color2;
        public uint Grade;
        public float Kmh;
        public float Mitron;
        public float MitronCapacity;

        public float MitronEfficiency;
        public bool SBBOn;
        public uint SlotType;
        
        public ulong CharacterId;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(CarId);
            writer.Write(CarType);
            writer.Write(BaseColor);
            writer.Write(Grade);
            writer.Write(SlotType);
            writer.Write(AuctionCnt);
            writer.Write(Mitron);
            writer.Write(Kmh);
            writer.Write(Color);
            writer.Write(Color2);
            writer.Write(MitronCapacity);
            writer.Write(MitronEfficiency);
            writer.Write(AuctionOn);
            writer.Write(SBBOn);
        }
        
        public void ReadFromDb(IDataRecord reader)
        {
            AuctionCnt = Convert.ToUInt32(reader["auctionCount"]);
            AuctionOn = false;
            BaseColor = Convert.ToUInt32(reader["baseColor"]);
            CarId = Convert.ToUInt32(reader["CID"]);
            CharacterId = Convert.ToUInt64(reader["CharID"]);
            CarType = Convert.ToUInt32(reader["carType"]);
            Color = Convert.ToUInt32(reader["color"]);
            Color2 = 0;
            Grade = Convert.ToUInt32(reader["grade"]);
            Kmh = (float) Convert.ToDouble(reader["kmh"]);
            Mitron = (float) Convert.ToDouble(reader["mitron"]);
            MitronCapacity = (float) Convert.ToDouble(reader["mitronCapacity"]);
            MitronEfficiency = (float) Convert.ToDouble(reader["mitronEfficiency"]);
            SBBOn = false;
            SlotType = Convert.ToUInt32(reader["slotType"]);
        }

        public void WriteToDb(ref InsertCommand cmd)
        {
            cmd.Set("auctionCount", AuctionCnt);
            //cmd.Set("auctionOn", vehicle.AuctionOn);
            cmd.Set("baseColor", BaseColor);
            //cmd.Set("CID", vehicle.CarID);
            cmd.Set("carType", CarType);
            cmd.Set("color", Color);
            //cmd.Set("color2", vehicle.Color2);
            cmd.Set("grade", Grade);
            cmd.Set("kmh", Kmh);
            cmd.Set("mitron", Mitron);
            cmd.Set("mitronCapacity", MitronCapacity);
            cmd.Set("mitronEfficiency", MitronEfficiency);
            //cmd.Set("SSBOn", vehicle.SSBOn);
            cmd.Set("slotType", SlotType);
            cmd.Execute();
        }
        
        public void WriteToDb(ref UpdateCommand cmd)
        {
            cmd.Set("auctionCount", AuctionCnt);
            //cmd.Set("auctionOn", vehicle.AuctionOn);
            cmd.Set("baseColor", BaseColor);
            //cmd.Set("CID", vehicle.CarID);
            cmd.Set("carType", CarType);
            cmd.Set("color", Color);
            //cmd.Set("color2", vehicle.Color2);
            cmd.Set("grade", Grade);
            cmd.Set("kmh", Kmh);
            cmd.Set("mitron", Mitron);
            cmd.Set("mitronCapacity", MitronCapacity);
            cmd.Set("mitronEfficiency", MitronEfficiency);
            //cmd.Set("SSBOn", vehicle.SSBOn);
            cmd.Set("slotType", SlotType);
            cmd.Execute();
        }
    }
}