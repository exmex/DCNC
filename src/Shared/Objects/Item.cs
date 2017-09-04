using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Runtime.CompilerServices;
using Shared.Database;
using Shared.Network;
using Shared.Util;

namespace Shared.Objects
{
    public class ItemMod : BinaryWriterExt.ISerializable
    {
        public Item Item;
        public int State;
        
        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(Item);            
            writer.Write(State);            
        }
    }
    
    public class Item : BinaryWriterExt.ISerializable
    {
        public int DbId;
        
        public uint CarId;
        public ushort State;
        public ushort Slot;
        public uint StackNum;
        public uint LastCarId;
        public uint AssistA;
        public uint AssistB;
        public uint AssistC;
        public uint AssistD;
        public uint AssistE;
        public uint AssistF;
        public uint AssistG;
        public uint AssistH;
        public uint AssistI;
        public uint AssistJ;
        public uint Box;
        public uint Belonging;
        public int Upgrade;
        public int UpgradePoint;
        public uint ExpireTick;
        public float Durability;
        public int TableIndex;
        public uint InventoryIndex;
        public int Random;
        public string Id;
        public ulong CharacterId;


        public static Item ReadFromDb(DbDataReader reader)
        {
            var item = new Item
            {
                DbId = Convert.ToInt32(reader["Id"]),
                CarId = Convert.ToUInt32(reader["CarId"]),
                State = Convert.ToUInt16(reader["State"]),
                Slot = Convert.ToUInt16(reader["Slot"]),
                StackNum = Convert.ToUInt32(reader["StackNum"]),
                AssistA = Convert.ToUInt32(reader["AssistA"]),
                AssistB = Convert.ToUInt32(reader["AssistB"]),
                AssistC = Convert.ToUInt32(reader["AssistC"]),
                AssistD = Convert.ToUInt32(reader["AssistD"]),
                AssistE = Convert.ToUInt32(reader["AssistE"]),
                AssistF = Convert.ToUInt32(reader["AssistF"]),
                AssistG = Convert.ToUInt32(reader["AssistG"]),
                AssistH = Convert.ToUInt32(reader["AssistH"]),
                AssistI = Convert.ToUInt32(reader["AssistI"]),
                AssistJ = Convert.ToUInt32(reader["AssistJ"]),
                Box = Convert.ToUInt32(reader["Box"]),
                Belonging = Convert.ToUInt32(reader["Belonging"]),
                Upgrade = Convert.ToInt32(reader["Upgrade"]),
                UpgradePoint = Convert.ToInt32(reader["UpgradePoint"]),
                ExpireTick = 0,
                Durability = Convert.ToSingle(reader["Durability"]),
                TableIndex = Convert.ToInt32(reader["TableIndex"]),
                InventoryIndex = Convert.ToUInt32(reader["InventoryIndex"]),
                Random = Convert.ToInt32(reader["Random"]),
                CharacterId = Convert.ToUInt64(reader["CharacterId"])
            };

            return item;
        }

        public void WriteToDb(ref UpdateCommand updateCommand)
        {
            
        }

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(CarId);
            writer.Write(State);
            writer.Write(Slot);
            writer.Write(StackNum);
            writer.Write(LastCarId);
            writer.Write(AssistA);
            writer.Write(AssistB);
            writer.Write(AssistC);
            writer.Write(AssistD);
            writer.Write(AssistE);
            writer.Write(AssistF);
            writer.Write(AssistG);
            writer.Write(AssistH);
            writer.Write(AssistI);
            writer.Write(AssistJ);
            writer.Write(Box);
            writer.Write(Belonging);
            writer.Write(Upgrade);
            writer.Write(UpgradePoint);
            writer.Write(ExpireTick);
            writer.Write(Durability); // Only neo, -1
            writer.Write(0); // Unk1?!
            writer.Write(TableIndex);
            writer.Write(InventoryIndex);
            writer.Write(Random);
        }
    }

    /*public class IUnit
    {
        public uint AssistA;
        public uint AssistB;
        public uint Belonging;
        public uint Box;

        public uint ExpireTick;

        public int Random;

        /*
        struct XiStrItemUnit
        {
          int StackNum;
          int Random;
          unsigned int AssistA;
          unsigned int AssistB;
          unsigned int Box;
          unsigned int Belonging;
          int Upgrade;
          int UpgradePoint;
          unsigned int ExpireTick;
        }
        /
        public int StackNum;

        public int Upgrade;
        public int UpgradePoint;
    }

    public class ItemData
    {
        public ushort Slot;
        /*
        struct $46506E0D494CF120A19388EB37177777
        {
          unsigned __int16 State;
          unsigned __int16 Slot;
        };
        union $90E2572CC35A071924DAD0BC1A98978B
        {
          $46506E0D494CF120A19388EB37177777 __s0;
                unsigned int StateVar;
            };
        /

        public ushort State;

        public uint StateVar;
    }

    public class Item : BinaryWriterExt.ISerializable
    {
        /*
        struct XiStrMyItem
        {
          unsigned int CarID;
          $90E2572CC35A071924DAD0BC1A98978B Itm;
          XiStrItemUnit ItemUnit;
          unsigned int TableIdx;
          unsigned int InvenIdx;
        };
        /
        public uint CarID;

        public uint InvenIdx;

        public ItemData Itm;
        public IUnit iunit;

        public uint TableIdx;

        public Item()
        {
            Itm = new ItemData();
            iunit = new IUnit();
        }

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(CarID);

            writer.Write(Itm.State);
            writer.Write(Itm.Slot);
            writer.Write(Itm.StateVar);

            writer.Write(iunit.StackNum);
            writer.Write(iunit.Random);

            writer.Write(iunit.AssistA);
            writer.Write(iunit.AssistB);

            writer.Write(iunit.Box);
            writer.Write(iunit.Belonging);

            writer.Write(iunit.Upgrade);
            writer.Write(iunit.UpgradePoint);

            writer.Write(iunit.ExpireTick);


            writer.Write(TableIdx);
            writer.Write(InvenIdx);

            Log.Debug("Bufferlength: " + writer.GetBuffer().Length);
        }
    }*/
}