using System;
using System.IO;

namespace TdfReader
{
    public class TDFFile
    {
        public class sBITMAP
        {
            public short bfType;
            public uint bfSize;
            public short bfReserved1;
            public short bfReserved2;
            public uint bfOffBits;
        }

        public class sVersion
        {
            public ushort major;
            public ushort minor;
        }

        public class sDate
        {
            public ushort Year;
            public char Month;
            public char Day;
        }

        public class sHeader
        {
            public sDate date;
            public uint Flag;
            public uint Offset;
            public uint Col;
            public uint Row;
        }

        public sBITMAP bitmap;
        public sVersion version;
        public sHeader header;
        public int[] dataTable;
        public byte[] resTable;

        public TDFFile()
        {
            bitmap = new sBITMAP();
            version = new sVersion();
            header = new sHeader();
            header.date = new sDate();
        }

        public void Read(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                bitmap.bfType = reader.ReadInt16();
                bitmap.bfSize = reader.ReadUInt32();
                bitmap.bfReserved1 = reader.ReadInt16();
                bitmap.bfReserved2 = reader.ReadInt16();
                bitmap.bfOffBits = reader.ReadUInt32();
                if (bitmap.bfType == 19778)
                    reader.ReadBytes((int) bitmap.bfSize - 14);

                version.major = reader.ReadUInt16();
                version.minor = reader.ReadUInt16();

                header.date.Year = reader.ReadUInt16();
                header.date.Month = reader.ReadChar();
                header.date.Day = reader.ReadChar();

                header.Flag = reader.ReadUInt32();
                header.Offset = reader.ReadUInt32();
                header.Col = reader.ReadUInt32();
                header.Row = reader.ReadUInt32();
                
                dataTable = new int[header.Col * header.Row];
                for (int i = 0; i < (header.Col * header.Row); i++)
                    dataTable[i] = reader.ReadInt32();

                resTable = new byte[header.Offset - (header.Col * 4 * header.Row + 24)];
                for (long i = 0; i < header.Offset - (header.Col * 4 * header.Row + 24); i++)
                    resTable[i] = reader.ReadByte();

                Console.WriteLine("DataTable size: " + dataTable.Length + ", ResTable size: " +
                                  resTable.Length);
            }
        }

        public static string GetColumnName(int column, string fileName)
        {
            switch (fileName)
            {
                case "ItemServer.tdf":
                case "ItemClient.tdf":
                    if (ItemColumns.Length > column)
                        return ItemColumns[column];
                break;

                case "QuestServer.tdf":
                case "QuestClient.tdf":
                    if (QuestColumns.Length > column)
                        return QuestColumns[column];
                break;
            }

            return column.ToString();
        }

        // TODO: More column names :)

        private static readonly string[] QuestColumns = {
            // TODO: Column count doesn't match sometimes
            "Quest ID",
            "Quest ID",
            "Prev Quest ID",
            "Event",
            "Need Level",
            "Need Level Percent",
            "Give Post",
            "Title",
            "End Post",
            "Place",
            "Place",
            "Place",
            "Place",
            "Place",
            "Crash Time",
            "Crash Time",
            "Crash Time",
            "Crash Time",
            "Crash Time",
            "Time Limit",
            "Time Limit",
            "Time Limit",
            "Time Limit",
            "Time Limit",
            "Min Speed",
            "Min Speed",
            "Min Speed",
            "Min Speed",
            "Min Speed",
            "Min Speed",
            "Max Speed",
            "Max Speed",
            "Max Speed",
            "Max Speed",
            "Max Speed",
            "Max Speed",
            "Min Ladius",
            "Min Ladius",
            "Min Ladius",
            "Min Ladius",
            "Min Ladius",
            "Max Ladius",
            "Max Ladius",
            "Max Ladius",
            "Max Ladius",
            "Max Ladius",
            "Quest Path 1",
            "Quest Path 2",
            "Car 1",
            "Car 2",
            "Clear Quest ID",
            "Count",
            "Reward Exp",
            "Reward Money",
            "Item 1",
            "Item 2",
            "Item 3"
        };

        private static readonly string[] ItemColumns = {
            "Type",
            "Set Type",
            "ID Name",
            "Group ID",
            "Name",
            "Grade",
            "Required Level",
            "Value",
            "Min",
            "Max",
            "Cost",
            "Sell",
            "Assist Num",
            "Assist Field",
            "Belong",
            "Next ID",
            "Shop",
            "Trade",
            "Auction",
            "Set Rate",
            "Set Desc",
            "Set Assist",
            "Time",
            "Jewel Type"
        };
    }
}