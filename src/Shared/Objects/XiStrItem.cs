using System;
using System.Collections.Generic;
using System.IO;
using Shared.Util;

namespace Shared.Objects
{
    public class XiStrItem
    {
        public uint Type;
        public string ID;
        public string GroupID; // 56
        public string Name; // 56
        public uint Grade;
        public uint ReqLevel;
        public uint Value;
        public int Min;
        public int Max;
        public long Cost;
        public uint Sell;
        public uint Time;
        public int AssistNum;
        public string AssistField; // 56
        public string NextID;
        public uint RoundNum;
        public uint Belong;
        //XiStrItemUseInfo UseInfo;
        //XiStrItem *NextItemPtr;
        public uint TableIdx;
        public int HeatState;
        public bool Shop;
        public bool Trade;
        public bool Auction;
        public uint SetType;
        public uint SetRate;
        //char SetDesc[160];
        public string SetDesc;
        //XiStrAssist *SetAssist;
        public uint TransType;
        public uint UpgradeType;
        public uint JewelType;
        //std::vector<AssistGroup *,std::allocator<AssistGroup *> > AssistList;
        
        public static Dictionary<uint, XiStrItem> LoadFromTdf(TdfReader tdfReader)
        {
            var itemList = new Dictionary<uint, XiStrItem>();
            using (var reader = new BinaryReaderExt(new MemoryStream(tdfReader.ResTable)))
            {
                for (var row = 0; row < tdfReader.Header.Row; row++)
                {
                    var item = new XiStrItem();
                    reader.ReadUnicode(); // Empty
                    item.Type = ItemTypeStringToVar(reader.ReadUnicode()); // Type
                    item.SetType = ItemSetTypeStrToVar(reader.ReadUnicode()); // SetType
                    item.ID = reader.ReadUnicode(); // Id Name
                    item.GroupID = reader.ReadUnicode();
                    item.Name = reader.ReadUnicode();
                    reader.ReadUnicode(); // ????
                    item.Grade = ItemGradeCharToVar(reader.ReadUnicode());
                    item.ReqLevel = Convert.ToUInt16(reader.ReadUnicode());
                    reader.ReadUnicode(); // ????
                    item.Value = Convert.ToUInt16(reader.ReadUnicode());
                    item.Min = Convert.ToUInt16(reader.ReadUnicode());
                    item.Max = Convert.ToUInt16(reader.ReadUnicode());
                    item.Cost = Convert.ToInt64(reader.ReadUnicode());
                    item.Sell = Convert.ToUInt16(reader.ReadUnicode());
                    item.NextID = reader.ReadUnicode();
                    item.Shop = reader.ReadUnicode().ToLower() == "true";
                    item.Trade = reader.ReadUnicode().ToLower() == "true";
                    item.Auction = reader.ReadUnicode().ToLower() == "true";
                    var setRate = reader.ReadUnicode();
                    if(setRate != "n/a")
                        item.SetRate = Convert.ToUInt16(setRate);
                    else
                        item.SetRate = 0;
                    
                    item.SetDesc = reader.ReadUnicode();
                    reader.ReadUnicode(); // SetAssist
                    //__that.SetAssist = XiAssistTable::GetAssistByID(v6, v12);
                    
                    item.Time = Convert.ToUInt16(reader.ReadUnicode());
                    itemList.Add((uint)row, item);
                }
            }
            return itemList;
        }
        
        public static uint ItemGradeCharToVar(string gradeStr)
        {
            switch (gradeStr)
            {
                case "N":
                    return 0x20000;
                case "S":
                    return 131073;
                case "H":
                    return 131074;
                case "E":
                    return 131075;
                case "FN":
                    return 131076;
                case "FS":
                    return 131077;
                case "FH":
                    return 131078;
                case "FE":
                    return 131079;
                case "ON":
                    return 131080;
                case "OS":
                    return 131081;
                case "OH":
                    return 131082;
                case "OE":
                    return 131083;
            }
            return 0;
        }

        public static uint ItemSetTypeStrToVar(string strType)
        {
            switch (strType)
            {
                case "kayla":
                    return 1;
                case "yuki":
                    return 2;
                case "hyeyoung":
                    return 3;
                case "erina":
                    return 4;
                case "duffy":
                    return 5;
                case "mrh":
                    return 6;
                case "yahn":
                    return 7;
                case "branton":
                    return 8;
            }
            return 0;
        }

        public static uint ItemTypeStringToVar(string strType)
        {
            switch (strType)
            {
                case "speed":
                    return 1;
                case "crash":
                    return 2;
                case "accel":
                    return 3;
                case "off_6C566C": // This is just the reference...
                    return 4;
                case "op_S":
                    return 6;
                case "op_C":
                    return 7;
                case "op_A":
                    return 8;
                case "op_B":
                    return 9;
                case "op_F":
                    return 10;
            }
            return 0;
        }
    }
}